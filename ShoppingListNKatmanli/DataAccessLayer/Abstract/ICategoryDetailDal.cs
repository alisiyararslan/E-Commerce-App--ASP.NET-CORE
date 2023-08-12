using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ICategoryDetailDal : IGenericDal<CategoryDetail>
    {
        CategoryDetail GetCategoryDetailByName(string name);
        List<CategoryDetail> GetCategoryDetailsBySubCategoryId(int subCategoryId);
    }
}
