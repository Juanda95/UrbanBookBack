using Application.Services.Interfaces;
using Domain.Entities.Parametros;
using Microsoft.Extensions.Logging;
using Persistence.UnitOfWork.Interface;

namespace Application.Services.BackGroundService
{
    public class DataBaseBackGroundService(IUnitOfWork unitOfWork,ILogger<DataBaseBackGroundService> logger) : IConfigBackGroundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DataBaseBackGroundService> _logger = logger;
        public async  Task<int> GetReminderUpdaterInterval()
        {
            int interval = 30;
            try
            {
                
                var ParameterRepository = _unitOfWork.GetRepository<Parameter>();
                var parameter = await ParameterRepository.FirstOrDefaultAsync(
                    parametro => parametro.TypeParameter.Equals("BackgroundServiceIntervals"),
                    parameter => parameter.Values);
                if (parameter != null)
                {
                    var value = parameter.Values.Find(v => v.Code.Equals("GetReminderUpdaterIntervalMinutes"))?.Name ?? string.Empty;
                    if (value != null)
                    {
                        interval = int.TryParse(value, out int result) ? result : interval;
                    }
                    
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Ocurrio un error consultando el intervalo");
            }
           

            return interval;

        }

        public async Task<Parameter?> GetReminderIntervalConfig()
        {
            
            try
            {

                var ParameterRepository = _unitOfWork.GetRepository<Parameter>();
                return await ParameterRepository.FirstOrDefaultAsync(
                    p => p.TypeParameter.Equals("ReminderEnabledBackgroundServiceConfig"),
                    p => p.Values);
                
                                   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error consultando el intervalo");
            }

            return null;

        }


    }
}
