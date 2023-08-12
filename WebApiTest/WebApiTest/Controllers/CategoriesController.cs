using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityLayer.DTOs;
using EntityLayer.Concrete;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public List<GetCategoryDTO> GetAllCategories()
        {
            List<Category> categories = _categoryService.GetListAll();

            List<GetCategoryDTO> categoryDTOs = categories.Select(category => new GetCategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();

            return categoryDTOs;
        }

        [HttpGet("getByName/{name:alpha}")]
        public Category GetCategory(string name)
        {
            var category = _categoryService.GetCategoryByName(name);

            if (category == null)
            {
                throw new Exception("NotFound");
            }

            return category;
        }

        [HttpGet("getById/{id:int}")]
        public Category GetCategory(int id)
        {
            var category = _categoryService.GetElementById(id);

            if (category == null)
            {
                throw new Exception("NotFound");
            }

            return category;
        }

        [HttpGet("getNameById/{id:int}")]
        public string GetCategoryName(int id)
        {
            var category = _categoryService.GetElementById(id);

            if (category == null)
            {
                throw new Exception("NotFound");
            }

            return category.Name;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(GetCategoryDTO dto)
        {

            if (ModelState.IsValid)
            {
                var categoryToUpdate = _categoryService.GetCategoryByName(dto.Name);
                if (categoryToUpdate == null)
                {
                    return NotFound();
                }

                categoryToUpdate.Name = dto.Name;

                _categoryService.Update(categoryToUpdate);

                return Ok("Category successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        [HttpPost("addCategory")]
        public async Task<ActionResult<AddCategoryDTO>> AddCategory(AddCategoryDTO category)
        {
            _categoryService.Insert(new Category()
            {
                Name = category.Name
            });

            return category;
        }

        [HttpDelete("deleteByName/{name:alpha}")]
        public async Task<IActionResult> DeleteCategory(string name)
        {
            var category = _categoryService.GetCategoryByName(name);
            if (category == null)
            {
                return NotFound();
            }

            _categoryService.Delete(category);

            return Ok("Category deleted successfully");
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _categoryService.GetElementById(id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryService.Delete(category);

            return Ok("Category deleted successfully");
        }
        private bool CategoryExists(string name)
        {
            var category = _categoryService.GetCategoryByName(name);

            return category != null;
        }
        private bool CategoryExists(int id)
        {
            var category = _categoryService.GetElementById(id);

            return category != null;
        }
    }
}
