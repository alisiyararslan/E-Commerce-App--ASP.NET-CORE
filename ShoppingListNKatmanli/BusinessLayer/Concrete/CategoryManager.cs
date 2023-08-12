using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {

        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public void Delete(Category t)
        {
            _categoryDal.Delete(t);
        }

        public Category GetElementById(int id)
        {
            return _categoryDal.GetElementById(id);
        }

        public List<Category> GetListAll()
        {
            return _categoryDal.GetListAll();
        }

        public void Insert(Category t)
        {
            _categoryDal.Insert(t);
        }

        public void Update(Category t)
        {
            _categoryDal.Update(t);
        }
        public Category GetCategoryByName(string name)
        {
            return _categoryDal.GetCategoryByName(name);
        }
    }
}
