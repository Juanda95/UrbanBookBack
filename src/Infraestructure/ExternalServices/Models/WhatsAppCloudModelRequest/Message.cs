namespace ExternalServices.Models.WhatsAppCloudModelRequest
{
    public class Message
    {
        public string? From { get; set; }
        public string? Id { get; set; }
        public string? Timestamp { get; set; }
        public string? Type { get; set; }
        public Text? Text { get; set; }
        public Interactive? Interactive { get; set; }
    }
}
