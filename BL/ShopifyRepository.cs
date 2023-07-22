using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using ShopifyData;
using ShopifyModel;
using ShopifySharp;

namespace BL
{
    public class ShopifyRepository : IShopifyRepo
    {
        public ShopifyRepository _shopifyService;
        private readonly ShopifyDBEntities _dbContext;
        public ShopifyRepository()
        {
            // Replace with your actual Shopify API credentials and store URL
            string shopifyApiKey = "0634901a1d5b5dae69ce3e807d1c2538";
            string shopifyPassword = "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";
            string shopifyStoreUrl = "quickstart-a7da73bb.myshopify.com";

            _shopifyService = new ShopifyRepository(shopifyApiKey, shopifyPassword, shopifyStoreUrl);
            _dbContext = new ShopifyDBEntities();
        }

        private readonly string _shopifyStoreUrl;
        private readonly string _shopifyApiKey;
        private readonly string _shopifyPassword;

        public ShopifyRepository(string shopifyApiKey, string shopifyPassword, string shopifyStoreUrl)
        {
            _shopifyStoreUrl = shopifyStoreUrl;
            _shopifyApiKey = shopifyApiKey;
            _shopifyPassword = shopifyPassword;

        }

        //working for Discount Rule or Price Rule
        public async Task<IEnumerable<PriceRule>> GetPriceRule()
        {
            var service = new PriceRuleService(_shopifyStoreUrl, _shopifyPassword);
            var productList = await service.ListAsync();

            var orders = productList.Items;
            return orders;
        }

        //working on create discount rule
        public async Task<PriceRule> CreateDiscountRule()
        {
            var service = new PriceRuleService(_shopifyStoreUrl, _shopifyPassword);
            var productList = await service.CreateAsync(new PriceRule()
            {
                //Id = 2,
                Title = "From DI",
                ValueType = "percentage",
                TargetType = "line_item",
                TargetSelection = "all",
                AllocationMethod = "across",
                Value = -10.0m,
                CustomerSelection = "all",
                OncePerCustomer = false,
                PrerequisiteSubtotalRange = new PrerequisiteValueRange()
                {
                    GreaterThanOrEqualTo = 40m
                },
                StartsAt = new DateTimeOffset(DateTime.Now)
            });

            return productList;
        }

        public async Task<PriceRuleDiscountCode> CreateDiscountCode(long priceRuleId/*, string discountCode, int? usageCount, DateTimeOffset? createdAt, DateTimeOffset? updatedAt*/)
        {
            var discountCodeService = new DiscountCodeService(_shopifyStoreUrl, _shopifyPassword);

            // Generate a random discount code
            string randomCode = GenerateRandomCode();

            var createdDiscountCode = await discountCodeService.CreateAsync(priceRuleId, new PriceRuleDiscountCode()
            {
                Code = randomCode,
                PriceRuleId = priceRuleId,
                // Set other properties as needed
            });

            return createdDiscountCode;
        }

        public string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomCode = new string(Enumerable.Repeat(chars, 8) // You can adjust the length of the code here (e.g., 8 characters).
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray());

            return randomCode;


        }












        public async Task<List<PriceRuleModel>> Index(/*long priceRuleId*/)
        {
            var crtPR = await _shopifyService.CreateDiscountRule();

            long? idd = crtPR.Id;

            long priceRuleId = (long)idd;

            var crtt = await _shopifyService.CreateDiscountCode(priceRuleId);


            var orders = await _shopifyService.GetPriceRule();
            //var orders2 = await _shopifyService.CreateDiscountRule();

            //List<PriceRuleTable> list = new List<PriceRuleTable>();
            var ordersModels = orders.Select(p => new PriceRuleModel
            {
                title = p.Title,
                target_type = p.TargetType,
                //Image = p.Images.FirstOrDefault()?.Src,
                target_selection = p.TargetSelection,
                allocation_method = p.AllocationMethod,
                value_type = p.ValueType,
                value = p.Value,
                once_per_customer = p.OncePerCustomer,
                usage_limit = p.UsageLimit,
                customer_selection = p.CustomerSelection,
                allocation_limit = p.AllocationLimit,
                starts_at = p.StartsAt,
                ends_at = p.EndsAt,
                created_at = p.CreatedAt,
                updated_at = p.UpdatedAt
            }).ToList();

            //var priceRuleEntities = ordersModels.Select(p => new PriceRuleTable
            //{
            //    title = p.title,
            //    target_type = p.target_type,
            //    // Map other properties accordingly
            //    created_at = p.created_at,
            //    updated_at = p.updated_at
            //}).ToList();

            //// Adding the PriceRuleTable entities to the DbContext and saving changes to the database
            //_dbContext.PriceRuleTable.AddRange(priceRuleEntities);
            //await _dbContext.SaveChangesAsync();

            foreach (var orderModel in ordersModels)
            {
                bool ordersExists = _dbContext.PriceRuleTable.Any(
                    p => p.created_at == orderModel.created_at);

                if (!ordersExists)
                {

                    var data = new PriceRuleTable
                    {
                        // Remove the assignment of AppId
                        title = orderModel.title,
                        target_type = orderModel.target_type,
                        target_selection = orderModel.target_selection,
                        allocation_method = orderModel.allocation_method,
                        value_type = orderModel.value_type,
                        value = orderModel.value,
                        once_per_customer = orderModel.once_per_customer,
                        usage_limit = orderModel.usage_limit,
                        customer_selection = orderModel.customer_selection,
                        allocation_limit = orderModel.allocation_limit,
                        starts_at = orderModel.starts_at,
                        ends_at = orderModel.ends_at,
                        created_at = orderModel.created_at,
                        updated_at = orderModel.updated_at
                    };


                    //var crtPR = await _shopifyService.CreateDiscountRule();

                    //long? idd = crtPR.Id;

                    //long priceRuleId = (long)idd;

                    //var crtt = await _shopifyService.CreateDiscountCode(priceRuleId);


                    _dbContext.PriceRuleTable.Add(data);
                    _dbContext.SaveChanges();
                }
            }
            return ordersModels;
        }



        //public async Task<List<PriceRuleModel>> Index(/*long priceRuleId*/)
        //{
        //    var crtPR = await _shopifyService.CreateDiscountRule();
        //    long? idd = crtPR.Id;
        //    long priceRuleId = (long)idd;
        //    var crtt = await _shopifyService.CreateDiscountCode(priceRuleId);

        //    var orders = await _shopifyService.GetPriceRule();

        //    var ordersModels = orders.Select(p => new PriceRuleModel
        //    {
        //        title = p.Title,
        //        target_type = p.TargetType,
        //        //Image = p.Images.FirstOrDefault()?.Src,
        //        target_selection = p.TargetSelection,
        //        allocation_method = p.AllocationMethod,
        //        value_type = p.ValueType,
        //        value = p.Value,
        //        once_per_customer = p.OncePerCustomer,
        //        usage_limit = p.UsageLimit,
        //        customer_selection = p.CustomerSelection,
        //        allocation_limit = p.AllocationLimit,
        //        starts_at = p.StartsAt,
        //        ends_at = p.EndsAt,
        //        created_at = p.CreatedAt,
        //        updated_at = p.UpdatedAt
        //    }).ToList();

        //    var existingData = _dbContext.PriceRuleTable.ToList();

        //    foreach (var orderModel in ordersModels)
        //    {
        //        bool ordersExists = existingData.Any(
        //            p => p.created_at == orderModel.created_at);

        //        if (!ordersExists)
        //        {
        //            var data = new PriceRuleTable
        //            {
        //                // Remove the assignment of AppId
        //                title = orderModel.title,
        //                target_type = orderModel.target_type,
        //                target_selection = orderModel.target_selection,
        //                allocation_method = orderModel.allocation_method,
        //                value_type = orderModel.value_type,
        //                value = orderModel.value,
        //                once_per_customer = orderModel.once_per_customer,
        //                usage_limit = orderModel.usage_limit,
        //                customer_selection = orderModel.customer_selection,
        //                allocation_limit = orderModel.allocation_limit,
        //                starts_at = orderModel.starts_at,
        //                ends_at = orderModel.ends_at,
        //                created_at = orderModel.created_at,
        //                updated_at = orderModel.updated_at
        //            };

        //            _dbContext.PriceRuleTable.Add(data);
        //            _dbContext.SaveChanges();
        //        }
        //    }
        //    return ordersModels;
        //}















    }
}
