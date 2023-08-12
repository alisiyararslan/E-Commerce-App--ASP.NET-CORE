using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IAddressDal : IGenericDal<Address>
    {
        List<Address> GetAddressesByUserId(int userId);

        Address GetFirstAddressByUserId(int userId);
    }
}
