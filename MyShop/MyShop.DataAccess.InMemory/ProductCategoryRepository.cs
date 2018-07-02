using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

/// <summary>
///  this entire class is no longer necessary as Generics (generic repository) has been implemented
/// </summary>

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cahe = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cahe["productCategories"] as List<ProductCategory>;
            if (productCategories == null)    // checks is there are no productCategories in the list
            {
                productCategories = new List<ProductCategory>(); // if no productCategories in list create a new list
            }
        }

        public void Commit()    // when user wants to add new productCategories to database they will first be cached "explicitly persist"
        {
            cahe["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory productCategory) // return a product a product update object 
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == productCategory.Id); // lamda to find and pas the product Id to the productToUpdate method

            if (productCategoryToUpdate != null) // checks if there is a product
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("ProductCategory Not Found"); // there are no productCategories to update
            }
        }

        public ProductCategory Find(string Id) // this method return Id
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id); // it only finds the Id and passes it to product

            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }
        }

        public IQueryable<ProductCategory> Collection() // returns a list of Products
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id) // deltes a product by its Id
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == Id); // finds the product Id to pass it to the delete method

            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("ProductCategory Not Found");
            }
        }
    }
}
