using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DNegocio;
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
                        // Obtener info del negocio del usuario
                        var negocioRepository = unitOfWork.GetRepository<Negocio>();
                        var negocio = await negocioRepository.FirstOrDefaultAsync(n => n.NegocioId == usuario.NegocioId);

                        // Generar token con claims completos
                        var roles = usuario.Perfiles.Select(p => p.Rol).ToList();
                        var token = GenerateJwtToken(usuario.Email, usuario.UsuarioId, usuario.NegocioId, roles);
                        var UsuarioResponse = mapper.Map<UsuarioDTOResponse>(usuario);

                        var UsuarioSesion = new LoginDTOResponse
                        {
                            Token = token,
                            Usuario = UsuarioResponse,
                            NegocioId = negocio?.NegocioId ?? 0,
                            NegocioNombre = negocio?.Nombre ?? string.Empty,
                            NegocioSlug = negocio?.Slug ?? string.Empty
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

        private string GenerateJwtToken(string username, int usuarioId, int negocioId, List<string> roles)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new("UsuarioId", usuarioId.ToString()),
                new("NegocioId", negocioId.ToString())
            };

            // Agregar un claim por cada rol para que [Authorize(Roles = "...")] funcione
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
