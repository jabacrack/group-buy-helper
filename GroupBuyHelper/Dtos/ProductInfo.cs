namespace GroupBuyHelper.Dtos
{
    public record ProductInfo
    {
        public ProductInfo(int id, string name, double? price, int? amount)
        {
            Id = id;
            Name = name;
            Price = price;
            Amount = amount;
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public double? Price { get; init; }
        public int? Amount { get; init; }
    }
}