using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ItemManager : IItemService
    {
        private readonly IItemDal _itemDal;
        private readonly IUserDal _userDal;

        public ItemManager(IItemDal itemDal, IUserDal userDal)
        {
            _itemDal = itemDal;
            _userDal = userDal;

        }

        public void Delete(Item t)
        {
            _itemDal.Delete(t);
        }

        public Item GetElementById(int id)
        {
            return _itemDal.GetElementById(id);
        }

        public List<Item> GetListAll()
        {
            List<Item> items = _itemDal.GetListAll();
            List<Item> validateItems = new List<Item>();

            foreach(var item in items)
            {
                var user = _userDal.GetElementById(item.UserId);
                if (user.IsActive)
                {
                    validateItems.Add(item);
                }
            }

            return validateItems;
        }

        public void Insert(Item t)
        {
            _itemDal.Insert(t);
        }

        public void Update(Item t)
        {
            _itemDal.Update(t);
        }

        public Item GetItemByBrand(string brand)
        {
            return _itemDal.GetItemByBrand(brand);
        }

        public List<Item> GetItemsByUserId(int userId)
        {
            return _itemDal.GetItemsByUserId(userId);
        }

    }
}
