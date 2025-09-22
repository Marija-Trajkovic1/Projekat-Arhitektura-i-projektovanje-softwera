using System.ComponentModel.DataAnnotations;

namespace TaskIT.DTOs.NotificationDTOs
{
    public class NotificationDTO
    {
        public string MessageText { get; set; }
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }
        public string IsRead { get; set; }
    }
}
