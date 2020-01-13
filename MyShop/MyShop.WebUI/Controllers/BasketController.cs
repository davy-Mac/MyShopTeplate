using MyShop.Core.Contracts;
using System.Web.Mvc;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        readonly IBasketService basketService; // local instance of IBasketService
        private IOrderService orderService; // local instance of IOrderService 

        public BasketController(IBasketService BasketService, IOrderService OrderService) // constructor to inject BasketService
        {
            this.basketService = BasketService;
            this.orderService= OrderService;
        }
        // GET: Basket
        public ActionResult Index() // view of the list of basket items
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        // Checkout endpoint
        public ActionResult Checkout() // method that returns the Checkout view
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Order order) // this method doesn't need a view, is the logic to process the payment
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";

            // process payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new {OrderId = order.Id});
        }

        public ActionResult ThankYou(string OrderId) // method that return the ThankYou view
        {
            ViewBag.OrderId = OrderId;
            return View();
        }

    }
}
