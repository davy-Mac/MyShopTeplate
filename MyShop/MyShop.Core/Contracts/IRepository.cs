using MyShop.Core.Models;
using System.Linq;

namespace MyShop.Core.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Commit();
        void Insert(T t);
        void Update(T t);
        T Find(string Id);
        IQueryable<T> Collection();
        void Delete(string Id);
    }
}
