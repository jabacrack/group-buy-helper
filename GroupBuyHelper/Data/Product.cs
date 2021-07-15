using System.Collections.Generic;

namespace GroupBuyHelper.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public int? Amount { get; set; }
        public string Description { get; set; }

        public ProductList ProductList { get; set; }
        
        public ICollection<UserOrderItem> ConnectedOrderItems { get; set; }
    }
}