using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DUsuario;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.UnitOfWork.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper) : IAuthService
    {
        public async Task<Response<LoginDTOResponse>> Authenticate(LoginDTORequest DataLogin)
        {
			try
			{
				using (unitOfWork)
				{
                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
					var usuario = await usuarioRepository.FirstOrDefaultAsync(
                        x => x.Email.Equals(DataLogin.Email) && 
                        x.Password.Equals(DataLogin.Password),
                        usuario => usuario.Perfiles);

					if (usuario != null)
					{
                        var token = GenerateJwtToken(DataLogin.Email);
                        var UsuarioResponse = mapper.Map<UsuarioDTOResponse>(usuario);
                        var UsuarioSesion = new LoginDTOResponse
                        {
                            Token = token,
                            Usuario = UsuarioResponse
                        };
                        return new Response<LoginDTOResponse>(UsuarioSesion, "Login exitoso");
                    }
                    return new Response<LoginDTOResponse>("Las credenciales proporcionadas son inválidas. Por favor, verifique su usuario y contraseña.", HttpStatusCode.NotFound);
                }
			}
			catch (Exception ex)
			{

                throw new Exception($"ocurrio un error en el Login {ex}");
            }
			
        }

        private string GenerateJwtToken(string username)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username)
            }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
