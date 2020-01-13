using System.Linq;
using MyShop.Core.Contracts;
using System.Web.Mvc;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IRepository<Customer> customers;// provides instance of IRepository<Customer>
        IBasketService basketService; // local instance of IBasketService
        IOrderService orderService; // local instance of IOrderService 

        public BasketController(IBasketService BasketService, IOrderService OrderService, IRepository<Customer> Customers) // constructor to inject BasketService, OrderService, CustomerRepository
        {
            this.basketService = BasketService;
            this.orderService= OrderService;
            this.customers = Customers;
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
        [Authorize]
        public ActionResult Checkout() // method that returns the Checkout view
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name); // to get the Identity of the User

            if (customer!=null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    Surname = customer.LastName,
                    ZipCode = customer.ZipCode
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order) // this method doesn't need a view, is the logic to process the payment
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created"; // this will set the OrderStatus field in db as "OrderCreated"
            order.Email = User.Identity.Name; // this will set the OrderEmail field in the db as the user

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
