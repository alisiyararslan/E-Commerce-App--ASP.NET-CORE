using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repository;
using DataAccessLayer.Contexts;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfUserDal : GenericRepository<User>, IUserDal //User işlemlerine erişebilmek için yaptık
    {
        
        public EfUserDal(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
       
    }
}
