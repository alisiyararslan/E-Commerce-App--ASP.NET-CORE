using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repository;
using DataAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;


namespace WebApiTest.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlCon"));
            });
        }

        //User Dependency Injection
        public static void ConfigureUserManager(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserManager>();
        }
        public static void ConfigureGenericDal(this IServiceCollection services)
        {
            services.AddScoped<IUserDal, EfUserDal>();
        }

        //Address Dependency Injection

        public static void ConfigureAddressManager(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressManager>();
        }

        public static void ConfigureAddressDal(this IServiceCollection services)
        {
            services.AddScoped<IAddressDal, EfAddressDal>();
        }

        //Category Dependency Injection

        public static void ConfigureCategoryManager(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryManager>();
        }

        public static void ConfigureCategoryDal(this IServiceCollection services)
        {
            services.AddScoped<ICategoryDal, EfCategoryDal>();
        }

        //SubCategory Dependency Injection

        public static void ConfigureSubCategoryManager(this IServiceCollection services)
        {
            services.AddScoped<ISubCategoryService, SubCategoryManager>();
        }

        public static void ConfigureSubCategoryDal(this IServiceCollection services)
        {
            services.AddScoped<ISubCategoryDal, EfSubCategoryDal>();
        }

        //CategoryDetail Dependency Injection

        public static void ConfigureCategoryDetailManager(this IServiceCollection services)
        {
            services.AddScoped<ICategoryDetailService, CategoryDetailManager>();
        }

        public static void ConfigureCategoryDetailDal(this IServiceCollection services)
        {
            services.AddScoped<ICategoryDetailDal, EfCategoryDetailDal>();
        }

        //Item Dependency Injection

        public static void ConfigureItemManager(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemManager>();
        }

        public static void ConfigureItemDal(this IServiceCollection services)
        {
            services.AddScoped<IItemDal, EfItemDal>();
        }

        //Orders Dependency Injection

        public static void ConfigureOrderManager(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderManager>();
        }

        public static void ConfigureOrderDal(this IServiceCollection services)
        {
            services.AddScoped<IOrderDal, EfOrderDal>();
        }

        //OrderDetail Dependency Injection

        public static void ConfigureOrderDetailManager(this IServiceCollection services)
        {
            services.AddScoped<IOrderDetailService, OrderDetailManager>();
        }

        public static void ConfigureOrderDetailDal(this IServiceCollection services)
        {
            services.AddScoped<IOrderDetailDal, EfOrderDetailDal>();
        }

        //Remainder Dependency Injection

        public static void ConfigureReminderManager(this IServiceCollection services)
        {
            services.AddScoped<IReminderService, ReminderManager>();
        }

        public static void ConfigureReminderDal(this IServiceCollection services)
        {
            services.AddScoped<IReminderDal, EfReminderDal>();
        }

        //FavoriteItemUser Dependency Injection
        public static void ConfigureFavoriteItemUserManager(this IServiceCollection services)
        {
            services.AddScoped<IFavoriteItemUserService, FavoriteItemUserManager>();
        }

        public static void ConfigureFavoriteItemUserDal(this IServiceCollection services)
        {
            services.AddScoped<IFavoriteItemUserDal, EfFavoriteItemUserDal>();
        }
    }
}
