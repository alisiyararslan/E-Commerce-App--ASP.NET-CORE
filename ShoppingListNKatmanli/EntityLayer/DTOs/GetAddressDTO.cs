using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetAddressDTO
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string AddressName { get; set; }

        public string CountryName { get; set; }

        public string CityName { get; set; }

        public string TownName { get; set; }

        public string DistrictName { get; set; }

        public int PostCode { get; set; }

        public string AddressText { get; set; }

        public static implicit operator GetAddressDTO(Address address)
        {
            return new GetAddressDTO
            {
                UserID =address.UserId,
                Id = address.Id,
                AddressName = address.AddressName,
                CountryName = address.CountryName,
                CityName = address.CityName,
                TownName = address.TownName,
                DistrictName = address.DistrictName,
                PostCode = address.PostCode,
                AddressText = address.AddressText
            };
        }


    }
}
