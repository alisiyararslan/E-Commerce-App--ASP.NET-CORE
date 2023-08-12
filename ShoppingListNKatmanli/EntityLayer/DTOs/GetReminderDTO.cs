using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetReminderDTO
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? Title { get; set; }

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public bool? IsRead { get; set; }
        public bool? IsSendMail { get; set; }

        public static implicit operator GetReminderDTO(Reminder reminder)
        {
            return new GetReminderDTO
            {
                Id = reminder.Id,
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
