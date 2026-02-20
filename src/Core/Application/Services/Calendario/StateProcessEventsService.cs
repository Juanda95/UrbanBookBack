using Application.DTOs.Request.Calendario;
using Application.DTOs.Response.Calendario;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces.Calendario;
using AutoMapper;
using Domain.Entities.DCalendario;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services.Calendario
{
    public class StateProcessEventsService(IUnitOfWork unitOfWork, IMapper mapper) : IStateProcessEventsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public Task<Response<StateProcessEventsResponse>> CreateAsync(StateProcessEventsDTORequest Request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<StateProcessEventsResponse>>> GetAll()
        {
            try
            {
                using (_unitOfWork)
                {
                    var StateProcessEventsRepository = _unitOfWork.GetRepository<StateProcessEvents>();
                    var StateProcessEvents = await StateProcessEventsRepository.GetAllAsyncThenInclude(
                        stateProcess => stateProcess
                        .Include(s => s.Eventos)
                            .ThenInclude(e => e.Cliente)
                        .Include(s => s.Eventos)
                            .ThenInclude(e => e.Usuario));
                    var StateProcessEventsResponse = _mapper.Map<List<StateProcessEventsResponse>>(StateProcessEvents);
                    return new Response<List<StateProcessEventsResponse>>(StateProcessEventsResponse, "Lista de eventos de estado", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<StateProcessEventsResponse>>(ex.Message, HttpStatusCode.InternalServerError);
            }
            
        }

        public Task<Response<StateProcessEventsResponse>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> UpdateAsync(StateProcessEventsDTOUpdateRequest UpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
