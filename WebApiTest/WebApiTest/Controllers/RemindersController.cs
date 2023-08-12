using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.DTOs;
using Humanizer;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {

        private readonly IReminderService _reminderService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public RemindersController(IReminderService reminderService, IUserService userService, IConfiguration configuration)
        {
            _reminderService = reminderService;
            _userService = userService;
            _configuration = configuration;
        }



        [HttpPost("addreminder")]
        public async Task<ActionResult<DefaultReminderDTO>> AddReminder(DefaultReminderDTO reminder)
        {
            _reminderService.Insert(new Reminder()
            {
                UserId = reminder.UserId,
                Title = reminder.Title,
                Date = reminder.Date,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
            });

            return reminder;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateReminder(GetReminderDTO dto)
        {

            if (ModelState.IsValid)
            {
                var reminderToUpdate = _reminderService.GetElementById(dto.Id);
                if (reminderToUpdate == null)
                {
                    return NotFound();
                }

                reminderToUpdate.UserId = dto.UserId;
                reminderToUpdate.Date = dto.Date;
                reminderToUpdate.Title = dto.Title;
                reminderToUpdate.Description = dto.Description;
                reminderToUpdate.IsRead = dto.IsRead;
                reminderToUpdate.IsSendMail = true;

                _reminderService.Update(reminderToUpdate);

                return Ok("Reminder successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteReminder(int reminderID)
        {
            var remainder = _reminderService.GetElementById(reminderID);
            if (remainder == null)
            {
                return NotFound();
            }

            _reminderService.Delete(remainder);

            return Ok("Remainder deleted successfully");
        }



        [HttpGet("get")]
        public Reminder GetReminder(int id)
        {
            var reminder = _reminderService.GetElementById(id);

            if (reminder == null)
            {
                throw new Exception("NotFound");
            }

            return reminder;
        }


        [HttpGet]
        public List<GetReminderDTO> GetAllReminders()
        {
            List<Reminder> reminders = _reminderService.GetListAll()/*.Where(r=>!r.IsSendMail).ToList()*/;

            List<GetReminderDTO> remindersDTOs = reminders.Select(reminder => new GetReminderDTO
            {
                Id = reminder.Id,
                Title = reminder.Title,
                Date = reminder.Date,
                UserId = reminder.UserId,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
                IsSendMail = reminder.IsSendMail,
            }).ToList();

            return remindersDTOs;
        }
        [HttpGet("GetNowReminders")]
        public async Task<List<GetReminderDTO>> GetNowReminders(int userId)
        {
            List<Reminder> reminders = _reminderService.GetListAll().Where(r => r.IsRead == false && r.UserId == userId && r.Date <= DateTime.Now).ToList();

            List<GetReminderDTO> remindersDTOs = reminders.Select(reminder => new GetReminderDTO
            {
                Id = reminder.Id,
                Title = reminder.Title,
                Date = reminder.Date,
                UserId = reminder.UserId,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
                IsSendMail = reminder.IsSendMail,


            }).ToList();
            var user = _userService.GetElementById(userId);

            foreach (var remind in remindersDTOs)
            {
                if (remind.IsSendMail == false)
                {
                    SendNotificationMailDTO dto = new SendNotificationMailDTO
                    {
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Title = remind.Title,
                        Description = remind.Description,

                    };
                    var httpClient = new HttpClient();
                    string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                    var response = await httpClient.PostAsJsonAsync(apiUrl + "Mail/SendNotificationMail", dto);


                    var reminder = _reminderService.GetElementById(remind.Id);
                    reminder.IsSendMail = true;
                    _reminderService.Update(reminder);
                }

            }

            return remindersDTOs;
        }
        //[HttpGet("IsSendMail")]
        //public async Task<IActionResult> IsSendMail(int id)
        //{
        //    var reminder = _reminderService.GetElementById(id);

        //    reminder.IsSendMail = true;
        //    _reminderService.Update(reminder);
        //    return Ok("Mail sended is succesful");

        //}

    }
}
