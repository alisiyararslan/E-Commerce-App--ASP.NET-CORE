using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using DataAccessLayer.Abstract;
using EntityLayer.DTOs;

namespace BusinessLayer.Concrete
{
    public class AddressManager : IAddressService
    {
        private readonly IAddressDal _addressDal;

        public AddressManager(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }
        public void Delete(Address t)
        {
            _addressDal.Delete(t);
        }

        public Address GetElementById(int id)
        {
            return _addressDal.GetElementById(id);
        }

        public List<Address> GetListAll()
        {
            return _addressDal.GetListAll();
        }

        public void Insert(Address t)
        {
            _addressDal.Insert(t);
        }

        public void Update(Address t)
        {
            _addressDal.Update(t);
        }


        public List<Address> GetAddressesByUserId(int userId)
        {
            List<Address> addresses = _addressDal.GetAddressesByUserId(userId);

            return addresses;
        }

        public Address GetFirstAddressByUserId(int userId)
        {
            Address address = _addressDal.GetFirstAddressByUserId(userId);

            return address;
        }

    }
}
