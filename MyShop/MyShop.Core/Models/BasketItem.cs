namespace MyShop.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public string BasketId { get; set; } // identifies the basket
        public string ProductId { get; set; } // identifies the each product    
        public int Quantity { get; set; } // the quantity of products in the basket
    }
}
