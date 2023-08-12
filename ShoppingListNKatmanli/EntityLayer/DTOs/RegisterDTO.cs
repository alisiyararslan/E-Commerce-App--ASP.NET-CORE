using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool Gender { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "Date")]
        public DateTime BirthDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public long PhoneNumber { get; set; }

        public static implicit operator User(RegisterDTO registerDTO)
        {
            return new User
            {
                UserName = registerDTO.UserName,
                Password = registerDTO.Password,
                Name = registerDTO.Name,
                Gender = registerDTO.Gender,
                BirthDate = registerDTO.BirthDate,
                RegisterDate = registerDTO.RegisterDate,
                PhoneNumber = registerDTO.PhoneNumber,
                IsActive = registerDTO.IsActive
            };
        }
    }
}
