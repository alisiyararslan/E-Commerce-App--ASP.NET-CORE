using DataAccessLayer.Concrete.Repository;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IItemService:IGenericService<Item>
    {
        Item GetItemByBrand(string name);

        List<Item> GetItemsByUserId(int userId);
    }
}
