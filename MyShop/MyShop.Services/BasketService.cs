using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; // to access http context and uses users cookies 

namespace MyShop.Services
{
    public class BasketService : IBasketService // implements the IBasketService defined in .Core/Contacts/
    {
        public const string BasketSessionName = "eCommerceBasket"; // to reference the cookie put in the users browser
        private readonly IRepository<Basket> basketContext;
        private IRepository<Product> productContext;

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext) // repository that takes the basket & product repos and use them internally
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull) // returns the basket object 'session'
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName); // reads the cookie as SessionName & assigns it to var cookie

            Basket basket = new Basket(); // creates a new basket

            if (cookie != null) // checks if there is an existing cookie
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId); // find the basket Id if it exists
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext); // creates a basket session string with the current httpContext 'cookie'
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext) // creates a new instance of basket
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit(); // writes the basket session in the database

            HttpCookie cookie = new HttpCookie(BasketSessionName); // creates a cookie
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1); // expiry interval for the cookie
            httpContext.Response.Cookies.Add(cookie); // sends response back to the user

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId) // this method adds to the basket
        {
            Basket basket = GetBasket(httpContext, true); // assigns the basket (session) to basket which most be already existent
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId); // see if there's already an item in the users basket

            if (item == null) 
            {
                item = new BasketItem() // initializes the object
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item); // adds the item to the basket
            }
            else
            {
                item.Quantity = item.Quantity + 1; // if there's already an product and another is added, increment quantity
            }
            basketContext.Commit(); // save to database
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId) // this method Removes Items form the basket
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext) // method to get the basket Items from DB uses BasketItemViewModel
        {
            Basket basket = GetBasket(httpContext, false); // fetches the basket, empty if there are no items in it

            if (basket != null) // if basket exist with items in it
            {
                var results = (from b in basket.BasketItems // query against the product table 
                               join p in productContext.Collection() on b.ProductId equals p.Id
                               select new BasketItemViewModel()
                               {
                                   Id = b.Id, // the id comes from the basket table
                                   Quantity = b.Quantity,
                                   ProductName = p.Name, // the name comes from the products table
                                   Image = p.Image,
                                   Price = p.Price
                               }
                    ).ToList();
                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0); // default values for the summary 0 items, 0 total 
            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems // sums up the NUMBER of total items in the basket
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems // sums up the total PRICE/COST of the items in the basket
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0; // if the basket is empty it will default to 0
                model.BasketTotal = basketTotal ?? decimal.Zero; // again if the basket is empty it will default to 0, 0

                return model;
            }
            else
            {
                return model; // this will return the empty model with default values
            }
        }
    }
}
