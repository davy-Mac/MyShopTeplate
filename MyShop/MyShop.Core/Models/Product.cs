using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Core.Models
{
    public class Product : BaseEntity
    {
        //public string Id { get; set; } // this no longer necesary as BaseEntity implements the Id

        [StringLength(20)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        //public Product() // this is handled in the BaseEntity
        //{
        //    this.Id = Guid.NewGuid().ToString(); // this sets the id as a new Guid and stored a string 
        //}
    }
}
