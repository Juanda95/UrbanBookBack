using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using Domain.Entities.DCalendario;
using Domain.Entities.Dcliente;
using Domain.Entities.DUsuario;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
    {
        private static readonly TimeSpan ColombiaOffset = TimeSpan.FromHours(-5);

        public async Task<Response<DashboardSummaryDTOResponse>> GetDashboardSummary(int year, int month)
        {
            try
            {
                using (unitOfWork)
                {
                    var inicioMes = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
                    var finMes = inicioMes.AddMonths(1);
                    var inicioMesAnterior = inicioMes.AddMonths(-1);

                    var eventoRepo = unitOfWork.GetRepository<Evento>();
                    var clienteRepo = unitOfWork.GetRepository<Cliente>();
                    var horarioRepo = unitOfWork.GetRepository<HorarioAtencion>();

                    // Obtener eventos del mes actual y anterior con sus relaciones
                    var eventosMes = await eventoRepo.FindAllAsync(
                        e => e.FechaInicio >= inicioMes && e.FechaInicio < finMes,
                        e => e.StateProcessEvent!, e => e.Cliente!);

                    var eventosMesAnterior = await eventoRepo.FindAllAsync(
                        e => e.FechaInicio >= inicioMesAnterior && e.FechaInicio < inicioMes,
                        e => e.StateProcessEvent!, e => e.Cliente!);

                    // Todos los eventos del negocio (para calcular clientes nuevos vs recurrentes)
                    var todosEventos = await eventoRepo.FindAllAsync(
                        e => e.FechaInicio < inicioMes,
                        e => e.Cliente!);

                    var horarios = await horarioRepo.FindAllAsync(h => h.Activo);

                    var summary = new DashboardSummaryDTOResponse();

                    // === TOP CARDS ===
                    CalcularIngresos(summary, eventosMes, eventosMesAnterior);
                    CalcularReservas(summary, eventosMes, eventosMesAnterior);
                    CalcularNuevosClientes(summary, eventosMes, eventosMesAnterior, todosEventos, inicioMes, inicioMesAnterior);
                    CalcularOcupacion(summary, eventosMes, horarios, inicioMes, finMes);

                    // === CHARTS ===
                    bool esMesActual = year == DateTime.UtcNow.Year && month == DateTime.UtcNow.Month;
                    CalcularIngresosPorDia(summary, eventosMes, inicioMes, finMes, esMesActual);
                    CalcularServiciosPopulares(summary, eventosMes);
                    CalcularHorasPico(summary, eventosMes);
                    CalcularClientesTipo(summary, eventosMes, todosEventos, inicioMes);
                    CalcularCancelaciones(summary, eventosMes);
                    CalcularClientesFrecuentes(summary, eventosMes);

                    // === INSIGHTS ===
                    GenerarInsights(summary);

                    return new Response<DashboardSummaryDTOResponse>(summary, "Dashboard obtenido con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el dashboard: {ex}");
            }
        }

        private static void CalcularIngresos(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes,
            List<Evento> eventosMesAnterior)
        {
            summary.IngresosMes = eventosMes
                .Where(e => e.StateProcessEvent?.State != "Cancelado")
                .Sum(e => e.Precio ?? 0);

            summary.IngresosMesAnterior = eventosMesAnterior
                .Where(e => e.StateProcessEvent?.State != "Cancelado")
                .Sum(e => e.Precio ?? 0);
        }

        private static void CalcularReservas(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes,
            List<Evento> eventosMesAnterior)
        {
            summary.ReservasMes = eventosMes.Count;
            summary.ReservasMesAnterior = eventosMesAnterior.Count;
        }

        private static void CalcularNuevosClientes(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes,
            List<Evento> eventosMesAnterior,
            List<Evento> eventosHistoricos,
            DateTime inicioMes,
            DateTime inicioMesAnterior)
        {
            // Un cliente es "nuevo" si su primer evento en todo el sistema cae en el mes actual
            var clientesConEventosPrevios = eventosHistoricos
                .Select(e => e.ClienteId)
                .ToHashSet();

            var clientesMes = eventosMes
                .Select(e => e.ClienteId)
                .Distinct()
                .ToList();

            summary.NuevosClientesMes = clientesMes
                .Count(c => !clientesConEventosPrevios.Contains(c));

            // Para el mes anterior: clientes que no tenían eventos antes del mes anterior
            var clientesAntesDelMesAnterior = eventosHistoricos
                .Where(e => e.FechaInicio < inicioMesAnterior)
                .Select(e => e.ClienteId)
                .ToHashSet();

            var clientesMesAnterior = eventosMesAnterior
                .Select(e => e.ClienteId)
                .Distinct()
                .ToList();

            summary.NuevosClientesMesAnterior = clientesMesAnterior
                .Count(c => !clientesAntesDelMesAnterior.Contains(c));
        }

        private static void CalcularOcupacion(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes,
            List<HorarioAtencion> horarios,
            DateTime inicioMes,
            DateTime finMes)
        {
            if (!horarios.Any())
            {
                summary.PorcentajeOcupacion = 0;
                return;
            }

            // Calcular minutos disponibles en el mes
            double totalMinutosDisponibles = 0;
            var fecha = inicioMes;
            while (fecha < finMes)
            {
                int diaSemana = (int)fecha.DayOfWeek;
                var horariosDelDia = horarios.Where(h => h.DiaSemana == diaSemana && h.Activo).ToList();
                foreach (var h in horariosDelDia)
                {
                    totalMinutosDisponibles += (h.HoraFin - h.HoraInicio).TotalMinutes;
                }
                fecha = fecha.AddDays(1);
            }

            // Calcular minutos reservados (solo eventos no cancelados)
            double totalMinutosReservados = eventosMes
                .Where(e => e.StateProcessEvent?.State != "Cancelado")
                .Sum(e => (e.FechaFin - e.FechaInicio).TotalMinutes);

            summary.PorcentajeOcupacion = totalMinutosDisponibles > 0
                ? Math.Round(totalMinutosReservados / totalMinutosDisponibles * 100, 1)
                : 0;
        }

        private static void CalcularIngresosPorDia(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes,
            DateTime inicioMes,
            DateTime finMes,
            bool esMesActual)
        {
            var eventosNoCancelados = eventosMes
                .Where(e => e.StateProcessEvent?.State != "Cancelado")
                .ToList();

            var agrupado = eventosNoCancelados
                .GroupBy(e => e.FechaInicio.Add(ColombiaOffset).Date)
                .ToDictionary(g => g.Key, g => g);

            var resultado = new List<IngresosPorDiaDTO>();
            var fecha = inicioMes;
            var hoy = DateTime.UtcNow.Add(ColombiaOffset).Date;
            while (fecha < finMes)
            {
                var fechaLocal = fecha.Add(ColombiaOffset).Date;

                // Si es el mes actual, solo mostrar hasta hoy
                if (esMesActual && fechaLocal > hoy)
                    break;

                agrupado.TryGetValue(fechaLocal, out var grupo);
                resultado.Add(new IngresosPorDiaDTO
                {
                    Fecha = fechaLocal.ToString("yyyy-MM-dd"),
                    Ingresos = grupo?.Sum(e => e.Precio ?? 0) ?? 0,
                    Reservas = grupo?.Count() ?? 0
                });
                fecha = fecha.AddDays(1);
            }

            summary.IngresosPorDia = resultado;
        }

        private static void CalcularServiciosPopulares(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes)
        {
            var eventosNoCancelados = eventosMes
                .Where(e => e.StateProcessEvent?.State != "Cancelado");

            summary.ServiciosPopulares = eventosNoCancelados
                .GroupBy(e => e.ServicioId.HasValue ? e.ServicioId.ToString() : e.Titulo)
                .Select(g => new ServicioPopularDTO
                {
                    Nombre = g.First().Titulo,
                    Cantidad = g.Count(),
                    Ingresos = g.Sum(e => e.Precio ?? 0)
                })
                .OrderByDescending(s => s.Cantidad)
                .Take(5)
                .ToList();
        }

        private static void CalcularHorasPico(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes)
        {
            var eventosActivos = eventosMes
                .Where(e => e.StateProcessEvent?.State != "Cancelado");

            summary.HorasPico = eventosActivos
                .GroupBy(e => (e.FechaInicio + ColombiaOffset).Hour)
                .Select(g => new HoraPicoDTO
                {
                    Hora = $"{g.Key:D2}:00",
                    Cantidad = g.Count()
                })
                .OrderBy(h => h.Hora)
                .ToList();
        }

        private static void CalcularClientesTipo(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes,
            List<Evento> eventosHistoricos,
            DateTime inicioMes)
        {
            var clientesConEventosPrevios = eventosHistoricos
                .Select(e => e.ClienteId)
                .ToHashSet();

            var clientesMesUnicos = eventosMes
                .Select(e => e.ClienteId)
                .Distinct()
                .ToList();

            int nuevos = clientesMesUnicos.Count(c => !clientesConEventosPrevios.Contains(c));
            int recurrentes = clientesMesUnicos.Count - nuevos;

            summary.ClientesTipo = new ClientesTipoDTO
            {
                Nuevos = nuevos,
                Recurrentes = recurrentes
            };
        }

        private static void CalcularCancelaciones(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes)
        {
            int canceladas = eventosMes.Count(e => e.StateProcessEvent?.State == "Cancelado");
            int completadas = eventosMes.Count - canceladas;

            summary.Cancelaciones = new CancelacionesDTO
            {
                Completadas = completadas,
                Canceladas = canceladas
            };
        }

        private static void CalcularClientesFrecuentes(
            DashboardSummaryDTOResponse summary,
            List<Evento> eventosMes)
        {
            var eventosNoCancelados = eventosMes
                .Where(e => e.StateProcessEvent?.State != "Cancelado");

            summary.ClientesFrecuentes = eventosNoCancelados
                .GroupBy(e => e.ClienteId)
                .Select(g =>
                {
                    var cliente = g.First().Cliente;
                    var nombre = cliente != null
                        ? $"{cliente.Nombres} {cliente.PrimerApellido}"
                        : $"Cliente #{g.Key}";
                    return new ClienteFrecuenteDTO
                    {
                        Nombre = nombre,
                        Visitas = g.Count(),
                        TotalGastado = g.Sum(e => e.Precio ?? 0)
                    };
                })
                .OrderByDescending(c => c.Visitas)
                .Take(5)
                .ToList();
        }

        private static void GenerarInsights(DashboardSummaryDTOResponse summary)
        {
            var insights = new List<string>();

            // Insight de ingresos
            if (summary.IngresosMesAnterior > 0)
            {
                var cambio = ((summary.IngresosMes - summary.IngresosMesAnterior) / summary.IngresosMesAnterior) * 100;
                if (cambio > 0)
                    insights.Add($"Tus ingresos crecieron {cambio:F1}% respecto al mes anterior");
                else if (cambio < 0)
                    insights.Add($"Tus ingresos bajaron {Math.Abs(cambio):F1}% respecto al mes anterior");
                else
                    insights.Add("Tus ingresos se mantienen igual al mes anterior");
            }
            else if (summary.IngresosMes > 0)
            {
                insights.Add("Este es tu primer mes con ingresos registrados");
            }

            // Insight de reservas
            if (summary.ReservasMesAnterior > 0)
            {
                var cambioReservas = ((double)(summary.ReservasMes - summary.ReservasMesAnterior) / summary.ReservasMesAnterior) * 100;
                if (cambioReservas > 0)
                    insights.Add($"Las reservas aumentaron {cambioReservas:F1}% este mes");
            }

            // Insight de nuevos clientes
            if (summary.NuevosClientesMes > 0)
                insights.Add($"Tienes {summary.NuevosClientesMes} nuevo(s) cliente(s) este mes");

            // Insight de hora pico
            var horaPico = summary.HorasPico.OrderByDescending(h => h.Cantidad).FirstOrDefault();
            if (horaPico != null)
                insights.Add($"Tu hora más activa es las {horaPico.Hora}");

            // Insight de ocupación
            if (summary.PorcentajeOcupacion >= 80)
                insights.Add("Tu agenda está casi llena, considera ampliar horarios");
            else if (summary.PorcentajeOcupacion < 30 && summary.ReservasMes > 0)
                insights.Add("Hay espacio para más citas, promociona tus servicios");

            summary.Insights = insights;
        }
    }
}
