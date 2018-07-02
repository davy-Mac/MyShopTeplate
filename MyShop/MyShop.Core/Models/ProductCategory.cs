namespace MyShop.Core.Models
{
    public class ProductCategory : BaseEntity
    {
        //public string Id { get; set; } // this is handled by the BaseEntity class
        public string Category { get; set; }

        //public ProductCategory() // this is handled by the BaseEntity class
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}
    }
}
