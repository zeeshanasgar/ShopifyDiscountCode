using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyModel
{
    public class PriceRuleModel
    {
        public int PriceRuleTableId { get; set; }
        public string title { get; set; }
        public string target_type { get; set; }
        public string target_selection { get; set; }
        public string allocation_method { get; set; }
        public string value_type { get; set; }
        public Nullable<decimal> value { get; set; }
        public Nullable<bool> once_per_customer { get; set; }
        public Nullable<long> usage_limit { get; set; }
        public string customer_selection { get; set; }
        public Nullable<int> allocation_limit { get; set; }
        public Nullable<System.DateTimeOffset> starts_at { get; set; }
        public Nullable<System.DateTimeOffset> ends_at { get; set; }
        public Nullable<System.DateTimeOffset> created_at { get; set; }
        public Nullable<System.DateTimeOffset> updated_at { get; set; }
    }
}
