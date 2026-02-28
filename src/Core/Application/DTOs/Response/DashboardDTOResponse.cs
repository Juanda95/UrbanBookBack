namespace Application.DTOs.Response
{
    public class DashboardSummaryDTOResponse
    {
        // --- TOP CARDS ---
        public decimal IngresosMes { get; set; }
        public decimal IngresosMesAnterior { get; set; }
        public int ReservasMes { get; set; }
        public int ReservasMesAnterior { get; set; }
        public int NuevosClientesMes { get; set; }
        public int NuevosClientesMesAnterior { get; set; }
        public double PorcentajeOcupacion { get; set; }

        // --- CHARTS ---
        public List<IngresosPorDiaDTO> IngresosPorDia { get; set; } = [];
        public List<ServicioPopularDTO> ServiciosPopulares { get; set; } = [];
        public List<HoraPicoDTO> HorasPico { get; set; } = [];
        public ClientesTipoDTO ClientesTipo { get; set; } = new();
        public CancelacionesDTO Cancelaciones { get; set; } = new();
        public List<ClienteFrecuenteDTO> ClientesFrecuentes { get; set; } = [];

        // --- INSIGHTS ---
        public List<string> Insights { get; set; } = [];
    }

    public class IngresosPorDiaDTO
    {
        public string Fecha { get; set; } = string.Empty;
        public decimal Ingresos { get; set; }
        public int Reservas { get; set; }
    }

    public class ServicioPopularDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Ingresos { get; set; }
    }

    public class HoraPicoDTO
    {
        public string Hora { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ClientesTipoDTO
    {
        public int Nuevos { get; set; }
        public int Recurrentes { get; set; }
    }

    public class CancelacionesDTO
    {
        public int Completadas { get; set; }
        public int Canceladas { get; set; }
    }

    public class ClienteFrecuenteDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public int Visitas { get; set; }
        public decimal TotalGastado { get; set; }
    }
}
