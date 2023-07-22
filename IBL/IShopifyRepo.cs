using ShopifyModel;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IShopifyRepo
    {
        Task<IEnumerable<PriceRule>> GetPriceRule();


        Task<PriceRule> CreateDiscountRule();

        Task<PriceRuleDiscountCode> CreateDiscountCode(long priceRuleId);

        string GenerateRandomCode();


        Task<List<PriceRuleModel>> Index();
    }
}
