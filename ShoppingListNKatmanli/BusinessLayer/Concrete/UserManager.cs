using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager:IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public void Delete(User t)
        {
            _userDal.Delete(t);
        }

        public User GetElementById(int id)
        {
            return _userDal.GetElementById(id);
        }

        public User GetElementByUsername(string username)
        {
            return _userDal.GetElementByUsername(username);
        }

        public List<User> GetListAll()
        {
            return _userDal.GetListAll();
        }

        public void Insert(User t)
        {
            _userDal.Insert(t);
        }

        public void Update(User t)
        {
            _userDal.Update(t);
        }

        public bool Login(string email, string password)
        {
            var user = _userDal.GetUserByEmailAndPassword(email, password);
            if (user != null)
            {
                // Giriş başarılı
                return true;
            }

            // Giriş başarısız
            return false;
        }

        public User GetUserByEmailAndPassword(string email,string password)
        {
            var user = _userDal.GetUserByEmailAndPassword(email, password);

            if (user != null)
                return user;
            else
                return null;
        }

        public User GetUserByEmail(string email)
        {
            var user = _userDal.GetUserByEmail(email);

            if (user != null)
                return user;
            else
                return null;
        }
    }
}
