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
    public class FavoriteItemUserManager : IFavoriteItemUserService
    {
        private readonly IFavoriteItemUserDal _favoriteItemUserDal;

        public FavoriteItemUserManager(IFavoriteItemUserDal favoriteItemUserDal)
        {
            _favoriteItemUserDal = favoriteItemUserDal;
        }
        void IGenericService<FavoriteItemUser>.Delete(FavoriteItemUser t)
        {
            _favoriteItemUserDal.Delete(t);
        }

        FavoriteItemUser IGenericService<FavoriteItemUser>.GetElementById(int id)
        {
            return _favoriteItemUserDal.GetElementById(id);
        }

        List<FavoriteItemUser> IGenericService<FavoriteItemUser>.GetListAll()
        {
            return _favoriteItemUserDal.GetListAll();
        }

        void IGenericService<FavoriteItemUser>.Insert(FavoriteItemUser t)
        {
            _favoriteItemUserDal.Insert(t);
        }

        void IGenericService<FavoriteItemUser>.Update(FavoriteItemUser t)
        {
            _favoriteItemUserDal.Update(t);
        }
    }
}
