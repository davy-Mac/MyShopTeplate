using MyShop.Core.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace MyShop.Core.Contracts
{
    public interface IBasketService
    {
        void AddToBasket(HttpContextBase httpContext, string productId); // these are the methods extracted "copied" from BasketService
        void RemoveFromBasket(HttpContextBase httpContext, string itemId);// these are the methods extracted "copied" from BasketService
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext);// these are the methods extracted "copied" from BasketService
        BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext);// these are the methods extracted "copied" from BasketService
    }
}
