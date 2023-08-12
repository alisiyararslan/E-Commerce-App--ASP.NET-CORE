using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class ResetPasswordDTO
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }
        public string PasswordAgain { get; set; }

        public string verifyCode { get; set; }
    }
}
