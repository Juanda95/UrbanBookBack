using Application.DTOs.DataEnvioNotificacion;
using Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExternalServices.Utils
{
    public class Util : IUtils
    {
        public object TextMessage(string message, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "text",
                text = new
                {
                    body = message
                }
            };
        }

        public object ImageMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "image",
                image = new
                {
                    link = url
                }
            };
        }

        public object AudioMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "audio",
                audio = new
                {
                    link = url
                }
            };
        }

        public object videoMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "video",
                video = new
                {
                    link = url
                }
            };
        }

        public object DocumentMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "document",
                document = new
                {
                    link = url
                }
            };
        }

        public object LocationMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "location",
                location = new
                {
                    latitude = "",
                    longitude = "",
                    name = "",
                    address = ""
                }
            };
        }

        public object ButtonsMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new
                    {
                        text = "Selecciona una opcion"
                    },
                    action = new
                    {
                        buttons = new List<object>
                        {
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "01",
                                    title = "comprar"
                                }
                            },
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "02",
                                    title = "Vender"
                                }
                            }
                        }
                    }
                }
            };
        }

        public object TemplateRecordatorio(DataEnvioWhatsAppDTO data)
        {
            string nombreCliente = data.Parameters?.FirstOrDefault(x => x.Nombre.Equals("nombreCliente"))?.Text ?? string.Empty;
            string FechaHora = data.Parameters?.FirstOrDefault(x => x.Nombre.Equals("fechaHora"))?.Text ?? string.Empty;
            string NombreEmpresa = data.Parameters?.FirstOrDefault(x => x.Nombre.Equals("nombreEmpresa"))?.Text ?? string.Empty;

            var headerComponent = new
            {
                type = "header",
                parameters = new[]
                {                   
                    new
                    {
                        type = "location",
                        location = new
                        {
                            latitude = data.DataEmpresa.Latitude,
                            longitude = data.DataEmpresa.Longitude,
                            name = data.DataEmpresa.Name,
                            address = data.DataEmpresa.Address
                        }
                    }
                }
            };

            var bodyComponent = new
            {
                type = "body",
                parameters = new[]
                {
                    new
                    {
                        type = "text",
                        text = nombreCliente
                    },
                    new
                    {
                        type = "text",
                        text = FechaHora
                    },
                    new
                    {
                        type = "text",
                        text = NombreEmpresa
                    }
                }
            };

            return new
            {
                messaging_product = "whatsapp",
                to = data.ToPhoneNumber,
                type = "template",
                template = new
                {
                    name = "recordatorio",
                    language = new
                    {
                        code = "es"
                    },
                    components = new object[]
                    {
                        headerComponent,
                        bodyComponent
                    }
                }
            };

        }
    }
}
