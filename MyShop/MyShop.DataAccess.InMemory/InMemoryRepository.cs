using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity // this is a generic class "T" is something that we will later refer to and implements BaseEntity
    {
        private ObjectCache cache = MemoryCache.Default;
        private List<T> items;
        private string className; // this is just a handle to how we are going to store our objects in the cache to differentiate each object

        public InMemoryRepository() // constructor to initialize our repository 
        {
            className = typeof(T).Name; // this is a reflection "typeof" will give us the actual name of an object that we pass in
            items = cache[className] as List<T>; //this initializes an items cache to check is any exist
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }
    }
}
