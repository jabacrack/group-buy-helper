using System.Collections.Generic;

namespace GroupBuyHelper.Data
{
    public class ProductList
    {
        public int Id { get; set; }
        public ApplicationUser Owner { get; set; }
        public ICollection<Product> Products { get; set; }

        public string Name { get; set; }
        public bool Closed { get; set; }
    }
}