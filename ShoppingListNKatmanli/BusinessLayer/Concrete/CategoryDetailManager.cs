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
    public class CategoryDetailManager : ICategoryDetailService
    {
        private readonly ICategoryDetailDal _categoryDetailDal;

        public CategoryDetailManager(ICategoryDetailDal categoryDetailDal)
        {
            _categoryDetailDal = categoryDetailDal;
        }
        public void Delete(CategoryDetail t)
        {
            _categoryDetailDal.Delete(t);
        }

        

        public CategoryDetail GetElementById(int id)
        {
            return _categoryDetailDal.GetElementById(id);
        }

        public List<CategoryDetail> GetListAll()
        {
            return _categoryDetailDal.GetListAll();
        }

        public void Insert(CategoryDetail t)
        {
            _categoryDetailDal.Insert(t);
        }

        public void Update(CategoryDetail t)
        {
            _categoryDetailDal.Update(t);
        }
        public CategoryDetail GetCategoryDetailByName(string name)
        {
            return _categoryDetailDal.GetCategoryDetailByName(name);
        }

        public List<CategoryDetail> GetCategoryDetailsBySubCategoryId(int subCategoryId)
        {
            return _categoryDetailDal.GetCategoryDetailsBySubCategoryId(subCategoryId);
        }
    }
}
