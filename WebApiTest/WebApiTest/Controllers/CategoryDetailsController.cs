using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityLayer.DTOs;
using EntityLayer.Concrete;
using BusinessLayer.Abstract;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryDetailsController : ControllerBase
    {
        private readonly ICategoryDetailService _categoryDetailService;

        public CategoryDetailsController(ICategoryDetailService categoryDetailService)
        {
            _categoryDetailService = categoryDetailService;
        }

        
        [HttpGet]
        public List<GetCategoryDetailDTO> GetAllCategoryDetails()
        {
            List<CategoryDetail> categoryDetails = _categoryDetailService.GetListAll();

            List<GetCategoryDetailDTO> categoryDetailDTOs = categoryDetails.Select(category => new GetCategoryDetailDTO
            {
                Id = category.Id,
                SubCategoryId = category.SubCategoryId,
                CategoryId = category.CategoryId,
                Name = category.Name
            }).ToList();

            return categoryDetailDTOs;
        }

         
        [HttpGet("subCategoryId/{id:int}")]
        public List<GetCategoryDetailDTO> GetCategoryDetailsBySubCategoryId(int id)
        {
            List<CategoryDetail> categoryDetails = _categoryDetailService.GetCategoryDetailsBySubCategoryId(id);

            List<GetCategoryDetailDTO> categoryDetailDTOs = categoryDetails.Select(category => new GetCategoryDetailDTO
            {
                Id = category.Id,
                SubCategoryId = category.SubCategoryId,
                CategoryId = category.CategoryId,
                Name = category.Name
            }).ToList();

            return categoryDetailDTOs;
        }

        
        [HttpGet("getByName/{name:alpha}")]
        public CategoryDetail GetCategoryDetail(string name)
        {
            var categoryDetail = _categoryDetailService.GetCategoryDetailByName(name);

            if (categoryDetail == null)
            {
                throw new Exception("NotFound");
            }

            return categoryDetail;
        }

        
        [HttpGet("getById/{id:int}")]
        public CategoryDetail GetCategoryDetail(int id)
        {
            var categoryDetail = _categoryDetailService.GetElementById(id);

            if (categoryDetail == null)
            {
                throw new Exception("NotFound");
            }

            return categoryDetail;
        }

        
        [HttpGet("getNameById/{id:int}")]
        public string GetCategoryDetailName(int id)
        {
            var categoryDetail = _categoryDetailService.GetElementById(id);

            if (categoryDetail == null)
            {
                throw new Exception("NotFound");
            }

            return categoryDetail.Name;
        }

        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategoryDetail(GetCategoryDetailDTO dto)
        {

            if (ModelState.IsValid)
            {
                var categoryDetailToUpdate = _categoryDetailService.GetCategoryDetailByName(dto.Name);
                if (categoryDetailToUpdate == null)
                {
                    return NotFound();
                }

                categoryDetailToUpdate.SubCategoryId = dto.SubCategoryId;
                categoryDetailToUpdate.CategoryId = dto.CategoryId;
                categoryDetailToUpdate.Name = dto.Name;

                _categoryDetailService.Update(categoryDetailToUpdate);

                return Ok("Category Detail successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        
        [HttpPost("addcategorydetail")]
        public async Task<ActionResult<AddCategoryDetailDTO>> AddCategoryDetail(AddCategoryDetailDTO categoryDetail)
        {
            _categoryDetailService.Insert(new CategoryDetail()
            {
                SubCategoryId = categoryDetail.SubCategoryId,
                CategoryId = categoryDetail.CategoryId,
                Name = categoryDetail.Name,
            });

            return categoryDetail;
        }

        
        [HttpDelete("deleteByName/{name:alpha}")]
        public async Task<IActionResult> DeleteCategoryDetail(string name)
        {
            var categoryDetail = _categoryDetailService.GetCategoryDetailByName(name);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            _categoryDetailService.Delete(categoryDetail);

            return Ok("Category Detail deleted successfully");
        }

        
        [HttpDelete("deleteById/{id:int}")]
        public async Task<IActionResult> DeleteCategoryDetail(int id)
        {
            var categoryDetail = _categoryDetailService.GetElementById(id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            _categoryDetailService.Delete(categoryDetail);

            return Ok("Category Detail deleted successfully");
        }
        private bool CategoryDetailExists(string name)
        {
            var categoryDetail = _categoryDetailService.GetCategoryDetailByName(name);

            return categoryDetail != null;
        }

        private bool CategoryDetailExists(int id)
        {
            var categoryDetail = _categoryDetailService.GetElementById(id);

            return categoryDetail != null;
        }
    }
}
