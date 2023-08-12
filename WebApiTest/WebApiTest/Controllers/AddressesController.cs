using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityLayer.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public List<GetAddressDTO> GetAllAddresses()
        {
            List<Address> addresses = _addressService.GetListAll();

            List<GetAddressDTO> addressDTOs = addresses.Select(address => new GetAddressDTO
            {
                Id = address.Id,
                UserID = address.UserId,
                AddressName = address.AddressName,
                CountryName = address.CountryName,
                CityName = address.CityName,
                TownName = address.TownName,
                DistrictName = address.DistrictName,
                PostCode = address.PostCode,
                AddressText = address.AddressText
            }).ToList();

            return addressDTOs;
        }

        [HttpGet("getAddressesByUserId")]
        public List<Address> GetAddressesByUserId(int userId)
        {
            List<Address> addresses = _addressService.GetAddressesByUserId(userId);

            List<Address> addressByUserId = addresses.Select(address => new Address
            {
                Id = address.Id,
                UserId = address.UserId,
                AddressName = address.AddressName,
                CountryName = address.CountryName,
                CityName = address.CityName,
                TownName = address.TownName,
                DistrictName = address.DistrictName,
                PostCode = address.PostCode,
                AddressText = address.AddressText
            }).ToList();

            return addressByUserId;
        }

        [HttpGet("getFirstAddressByUserId")]
        public Address GetFirstAddressByUserId(int userId)
        {
            Address address = _addressService.GetFirstAddressByUserId(userId);

            return address;
        }

        [HttpGet("get")]
        public Address GetAddress( int id)
        {
            var address = _addressService.GetElementById(id);

            if (address == null)
            {
                throw new Exception("NotFound");
            }

            return address;
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateAddress(GetAddressDTO dto)
        {

            if (ModelState.IsValid)
            {
                var addressToUpdate = _addressService.GetElementById(dto.Id);
                if (addressToUpdate == null)
                {
                    return NotFound();
                }

                addressToUpdate.AddressName = dto.AddressName;
                addressToUpdate.CountryName = dto.CountryName;
                addressToUpdate.CityName = dto.CityName;
                addressToUpdate.TownName = dto.TownName;
                addressToUpdate.DistrictName = dto.DistrictName;
                addressToUpdate.PostCode = dto.PostCode;
                addressToUpdate.AddressText = dto.AddressText;

                _addressService.Update(addressToUpdate);

                return Ok("Address successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        [HttpPost("addaddress")]
        public async Task<ActionResult<DefaultAddressDTO>> AddAddress(DefaultAddressDTO address)
        {
            _addressService.Insert(new Address()
            {
                UserId = address.UserID,
                AddressName = address.AddressName,
                CountryName = address.CountryName,
                CityName = address.CityName,
                TownName = address.TownName,
                DistrictName = address.DistrictName,
                PostCode = address.PostCode,
                AddressText = address.AddressText,
            });

            return address;
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAddress(int addressID)
        {
            var address = _addressService.GetElementById(addressID);
            if (address == null)
            {
                return NotFound();
            }

            _addressService.Delete(address);

            return Ok("User deleted successfully");
        }

        
    }
}
