using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class LoginDTOResponse
    {
        public string Token { get; set; } = string.Empty;
        
        public UsuarioDTOResponse Usuario { get; set; } = new UsuarioDTOResponse();

    }
}
