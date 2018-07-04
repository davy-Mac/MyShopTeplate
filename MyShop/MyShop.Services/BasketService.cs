using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Linq;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        public const string BasketSessionName = "eCommerceBasket";
        private readonly IRepository<Basket> basketContext;
        private IRepository<Product> productContext;

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            basketContext = BasketContext;
            productContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull) // returns the basket object
        {
            var cookie = httpContext.Request.Cookies.Get(BasketSessionName); // assigns the SessionName as cookie

            var basket = new Basket(); // creates a new basket
            if (cookie != null) // checks if there is an existing cookie
            {
                var basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId); // find the basket Id if it exists
                }
                else
                {
                    if (createIfNull) basket = CreateNewBasket(httpContext);
                }
            }

            else
            {
                if (createIfNull) basket = CreateNewBasket(httpContext);
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext) // creates a new instance of basket
        {
            var basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            var cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1); // expiry interval for the cookie
            httpContext.Response.Cookies.Add(cookie); // sends response back to the user

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId) // this method adds to the basket
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null) // checks if no items in basket
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }
            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId) // this method Removes Items form the basket
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (itemId != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }
    }
}
