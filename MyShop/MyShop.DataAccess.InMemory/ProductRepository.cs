using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cahe = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cahe["products"] as List<Product>;
            if(products == null)    // checks is there are no products in the list
            {
                products = new List<Product>(); // if no products in list create a new list
            }
        }

        public void Commit()    // when user wants to add new products to database they will first be cached "explicitly persist"
        {
            cahe["products"] = products;
        }

        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update(Product product) // return a product a product update object 
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id); // lamda to find and pas the product Id to the productToUpdate method

            if(productToUpdate != null) // checks if there is a product
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product Not Found"); // there are no products to update
            }
        }

        public Product Find(string Id) // this method return Id
        {
            Product product = products.Find(p => p.Id == Id); // it only finds the Id and passes it to product

            if(product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public IQueryable<Product> Collection() // returns a list of Products
        {
            return products.AsQueryable();
        }

        public void Delete(string Id) // deltes a product by its Id
        {
            Product productToDelete = products.Find(p => p.Id == Id); // finds the product Id to pass it to the delete method

            if(productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }
    }
}
