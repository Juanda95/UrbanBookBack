namespace Application.DTOs.DataEnvioNotificacion
{
    public class DataEnvioWhatsAppDTO
    {
        public DataEmpresaDTO DataEmpresa {  get; set; } = new DataEmpresaDTO(); 
        public List<ParametroDTO> Parameters { get; set; } = [];
        public string ToPhoneNumber { get; set; } = string.Empty;
    }
}
 