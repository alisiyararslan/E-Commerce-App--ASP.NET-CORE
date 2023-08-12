using DataAccessLayer.Abstract;
using DataAccessLayer.Contexts;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repository
{
    public class GenericRepository<T> : IGenericDal<T> where T : class, new()
    {
        private readonly DbContextOptions<AppDbContext> options;
        public GenericRepository(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }
        public void Delete(T t)
        {
        
            var context = new AppDbContext(options);
            context.Remove(t);
            context.SaveChanges();
        }

        public T GetElementById(int id)
        {
            
            using var context = new AppDbContext(options);
            return context.Set<T>().Find(id);
        }

        public User GetElementByUsername(string username)
        {
            using var context = new AppDbContext(options);
            return context.Users.FirstOrDefault(x => x.UserName == username);
        }

        public List<T> GetListAll()
        {
            
            using var context = new AppDbContext(options);
            return context.Set<T>().ToList();
        }

        public void Insert(T t)
        {
           
            using var context = new AppDbContext(options);
            context.Add(t);
            context.SaveChanges();
        }

        public void Update(T t)
        {
        
            using var context = new AppDbContext(options);
            context.Update(t);
            context.SaveChanges();
        }

        public List<Address> GetAddressesByUserId(int userId)
        {
            using var context = new AppDbContext(options);
            return context.Set<Address>().Where(x => x.UserId == userId).ToList();
        }
        public User GetUserByEmailAndPassword(string email, string password)
        {
            using var context = new AppDbContext(options);
            var user = context.Users.FirstOrDefault(x => x.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public User GetUserByEmail(string email)
        {
            using var context = new AppDbContext(options);
            return context.Users.FirstOrDefault(x => x.Email == email);
        }

        public Category GetCategoryByName(string name)
        {
            using var context = new AppDbContext(options);
            return context.Categories.FirstOrDefault(x => x.Name == name);
        }

        public SubCategory GetSubCategoryByName(string name)
        {
            using var context = new AppDbContext(options);
            return context.SubCategories.FirstOrDefault(x => x.Name == name);
        }

        public CategoryDetail GetCategoryDetailByName(string name)
        {
            using var context = new AppDbContext(options);
            return context.CategoryDetails.FirstOrDefault(x => x.Name == name);
        }

        public Item GetItemByBrand(string brand)
        {
            using var context = new AppDbContext(options);
            return context.Items.FirstOrDefault(x => x.Brand == brand);
        }

        public List<CategoryDetail> GetCategoryDetailsBySubCategoryId(int subCategoryId)
        {
            using var context = new AppDbContext(options);
            var categoryDetails = context.CategoryDetails.Where(x => x.SubCategoryId == subCategoryId).ToList();
            return categoryDetails;
        }

        public List<SubCategory> GetSubCategoriesByCategoryId(int categoryId)
        {
            using var context = new AppDbContext(options);
            var subCategories = context.SubCategories.Where(x => x.CategoryId == categoryId).ToList();
            return subCategories;
        }

        public List<Item> GetItemsByUserId(int userId)
        {
            using var context = new AppDbContext(options);
            return context.Set<Item>().Where(x => x.UserId == userId).ToList();
        }
        public List<Order> GetOrdersByUserId(int userId)
        {
            using var context = new AppDbContext(options);
            return context.Set<Order>().Where(x => x.UserId == userId).ToList();
        }
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            using var context = new AppDbContext(options);
            var orderDetails = context.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            return orderDetails;
        }

        public Order GetOrderByShareCode(string shareCode)
        {
            using var context = new AppDbContext(options);
            return context.Orders.FirstOrDefault(x => x.ShareCode == shareCode);

        }

        public Address GetFirstAddressByUserId(int userId)
        {
            using var context = new AppDbContext(options);
            return context.Addresses.First(x => x.UserId == userId);
        }

        public Order GetMainOrderByIsMain(int userId)
        {
            using var context = new AppDbContext(options);
            return context.Orders.FirstOrDefault(x => x.IsMain == true && x.UserId == userId);
        }
    }
}
