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
    public class UserDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool Gender { get; set; }

        [Column(TypeName = "Date")]
        public DateTime BirthDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public long PhoneNumber { get; set; }

        public bool isActive{ get; set; }

        public static implicit operator UserDTO(User User)
        {
            return new UserDTO()
            {
                Id = User.Id,
                UserName = User.UserName,
                Password = User.Password,
                Name = User.Name,
                Surname = User.Surname,
                Email = User.Email,
                Gender = User.Gender,
                PhoneNumber = User.PhoneNumber,
                BirthDate = User.BirthDate,
                RegisterDate = User.RegisterDate,
                isActive = User.IsActive
            };
        }
    }
}
