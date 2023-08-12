using DataAccessLayer.Contexts;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IUserDal:IGenericDal<User>//user data access layer interface
    {
        User GetUserByEmailAndPassword(string email, string password);
        User GetElementByUsername(string username);

        User GetUserByEmail(string email);

    }
}
