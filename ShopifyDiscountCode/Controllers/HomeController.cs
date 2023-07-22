using IBL;
using ShopifyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopifyDiscountCode.Controllers
{
    public class HomeController : Controller
    {
        readonly IShopifyRepo userRepository;
        public HomeController(IShopifyRepo employeeRepository)
        {
            userRepository = employeeRepository;
        }
        //public HomeController()
        //{
        //}
        public async Task<ActionResult> Index(PriceRuleModel priceRuleModel)
        {
            var items = await userRepository.Index();
            return View(items);
        }



    }
}