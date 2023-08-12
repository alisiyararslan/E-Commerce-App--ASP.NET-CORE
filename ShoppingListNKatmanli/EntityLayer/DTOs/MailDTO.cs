using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class MailDTO
    {
        public string Name { get; set; }
        public string SenderMail { get; set; }
       // public string ReceiverMail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


    }
}
