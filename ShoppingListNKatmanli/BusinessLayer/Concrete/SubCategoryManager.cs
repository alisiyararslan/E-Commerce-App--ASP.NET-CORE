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
    public class SubCategoryManager : ISubCategoryService
    {
        private readonly ISubCategoryDal _subCategoryDal;

        public SubCategoryManager(ISubCategoryDal subCategory)
        {
            _subCategoryDal = subCategory;
        }
        public void Delete(SubCategory t)
        {
            _subCategoryDal.Delete(t);
        }

        public SubCategory GetElementById(int id)
        {
            return _subCategoryDal.GetElementById(id);
        }

        public List<SubCategory> GetListAll()
        {
            return _subCategoryDal.GetListAll();
        }

        public void Insert(SubCategory t)
        {
            _subCategoryDal.Insert(t);
        }

        public void Update(SubCategory t)
        {
            _subCategoryDal.Update(t);
        }
        public SubCategory GetSubCategoryByName(string name)
        {
            return _subCategoryDal.GetSubCategoryByName(name);
        }

        public List<SubCategory> GetSubCategoriesByCategoryId(int categoryId)
        {
            return _subCategoryDal.GetSubCategoriesByCategoryId(categoryId);
        }
    }
}
