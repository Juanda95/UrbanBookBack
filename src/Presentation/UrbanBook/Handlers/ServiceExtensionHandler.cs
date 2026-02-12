using Application;
using ExternalServices;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Scheduler;
using System.Text;
namespace UrbanBook.Handlers
{
    public class ServiceExtensionHandler
    {
        public static void ServiceExtensionsConfig(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("cnUrbanBook") ?? string.Empty;
            var UriWhatsAppApi = builder.Configuration["WhatsApp:ApiUrl"] ?? string.Empty;
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty);


            builder.Services.AddPersistenceServices(connectionString);
            builder.Services.AddAplicationLayer();
            builder.Services.AddExternalServices(UriWhatsAppApi);
            builder.Services.AddSchedulerServices();


            //Middleware
            builder.Services.AddTransient<ValidationHandlerMiddleware>();

            //JWT
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
            {
                var signinKey = new SymmetricSecurityKey(key);
                var signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature);

                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = signinKey
                };

            });
        
        }
    }
}
