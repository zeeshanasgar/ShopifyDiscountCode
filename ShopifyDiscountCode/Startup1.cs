using BL;
using IBL;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;



namespace ShopifyDiscountCode
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            IShopifyRepo userRepository = new ShopifyRepository();
            //UnityConfig.RegisterComponents();
        }
    }
}
