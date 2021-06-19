namespace GroupBuyHelper.Data
{
    public class UserOrderItem
    {
        public int Id { get; set; }

        public ApplicationUser Owner { get; set; }

        public int ProductListId { get; set; }
        public ProductList ProductList { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }
    }
}