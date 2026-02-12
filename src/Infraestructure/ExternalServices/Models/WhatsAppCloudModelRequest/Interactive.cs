namespace ExternalServices.Models.WhatsAppCloudModelRequest
{
    public class Interactive
    {
        public string? Type { get; set; }
        public ListReply? List_Reply { get; set; }
        public ButtonReply? Button_Reply { get; set; }
    }
}
