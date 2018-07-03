using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Product> context;
        private IRepository<ProductCategory> productCategories;
        //ProductRepository context;
        //ProductCategoryRepository productCategories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext) //IRepository interface injected
        {
            context = productContext;
            productCategories = productCategoryContext;
        }

        public ActionResult Index(string Category = null)// "string Category=null" means it is optional if no element is passed it will be assumed as null
        {

            List<Product> products; // this is an empty list of products
            List<ProductCategory> categories = productCategories.Collection().ToList();

            if (Category == null)
            {
                products = context.Collection().ToList();
            }
            else
            {
                products = context.Collection().Where(p => p.Category == Category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCatogories = categories;

            return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
