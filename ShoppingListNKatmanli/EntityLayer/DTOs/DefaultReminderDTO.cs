using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class DefaultReminderDTO
    {
        public int? UserId { get; set; }

        public string? Title { get; set; }

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public bool? IsRead { get; set; }

        public static explicit operator DefaultReminderDTO(Reminder reminder)
        {
            return new DefaultReminderDTO
            {
                UserId = reminder.UserId,
                Title = reminder.Title,
                Date = reminder.Date,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
            };
        }
    }
}
