using MyShop.Core.Models;
using System.Data.Entity;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<Product> Products { get; set; } // the Products entity
        public DbSet<ProductCategory> ProductCategories { get; set; } // the ProductCategories entity in the DB
        public DbSet<Basket> baskets { get; set; } // the Basket entity in the DB
        public DbSet<BasketItem> BasketItems { get; set; } // the BasketItems in the DB
    }
}
