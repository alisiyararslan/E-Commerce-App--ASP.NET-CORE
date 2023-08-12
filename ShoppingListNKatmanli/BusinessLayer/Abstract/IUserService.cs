using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {

        bool Login(string email, string password);

        User GetElementByUsername(string username);

        User GetUserByEmailAndPassword(string email,string password);

        User GetUserByEmail(string email);
    }
}
