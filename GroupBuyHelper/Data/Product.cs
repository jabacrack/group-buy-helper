namespace GroupBuyHelper.Data
{
    public class Product
    {
        public int Id { get; set; }
        public ProductList ProductsList { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }

    }
}