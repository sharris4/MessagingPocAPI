using System.Collections.Generic;

namespace MessagingPocApi.Models
{
    public class FolderDto
    {
        public int? id { get; set; }
        public string name { get; set; }
        public List<MessageDto> messages { get; set; }
    }
}
