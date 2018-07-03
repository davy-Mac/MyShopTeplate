using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System.Data.Entity;
using System.Linq;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity // "T" is just a placeholder that we'll reference to later 
    {
        internal DataContext context;
        internal DbSet<T> dbSet;

        public SQLRepository(DataContext context) //Constructor that allows to pass in the DataContext internally
        {
            this.context = context;
            this.dbSet = context.Set<T>(); //sets the underlying table by referencing the table and passing in the model which is product or category
        }

        public IQueryable<T> Collection() // returns the dbSet
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached)
                dbSet.Attach(t);

            dbSet.Remove(t);
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            context.Entry(t).State = EntityState.Modified;
        }
    }
}
