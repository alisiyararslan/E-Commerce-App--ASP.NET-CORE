using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class ReminderDTO
    {
        public int? UserId { get; set; }

        public string? Title { get; set; }

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public bool? IsRead { get; set; }
        public bool? IsSendMail { get; set; }

        public static implicit operator ReminderDTO(Reminder reminder)
        {
            return new ReminderDTO
            {
                UserId = reminder.UserId,
                Title = reminder.Title,
                Date = reminder.Date,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
                IsSendMail = reminder.IsSendMail,
            };
        }
    }
}
