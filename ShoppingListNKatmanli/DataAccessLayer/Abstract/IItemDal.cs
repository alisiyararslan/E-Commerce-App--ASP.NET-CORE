using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IItemDal : IGenericDal<Item>
    {
        Item GetItemByBrand(string name);
        List<Item> GetItemsByUserId(int userId);

    }
}
