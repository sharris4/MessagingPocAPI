namespace MessagingPocApi.Models
{
    public class MessageDto
    {
        public int? id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}