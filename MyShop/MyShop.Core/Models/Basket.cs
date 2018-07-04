using System.Collections.Generic;

namespace MyShop.Core.Models
{
    public class Basket : BaseEntity
    {
        public virtual ICollection<BasketItem> BasketItems { get; set; } // this is a virtual ICollection, it will load all the items in the basket "lazy loading"

        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }
    }
}
