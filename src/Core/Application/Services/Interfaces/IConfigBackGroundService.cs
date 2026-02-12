using Domain.Entities.Parametros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IConfigBackGroundService
    {
        public Task<int> GetReminderUpdaterInterval();
        public Task<Parameter?> GetReminderIntervalConfig();
    }
}
