using Application.DTOs.DataEnvioNotificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUtils
    {
        public object TextMessage(string message, string number);
        public object ImageMessage(string url, string number);
        public object AudioMessage(string url, string number);
        public object videoMessage(string url, string number);
        public object DocumentMessage(string url, string number);
        public object LocationMessage(string number);
        public object ButtonsMessage(string number);
        public object TemplateRecordatorio(DataEnvioWhatsAppDTO dataEnvio); 
    }
}
