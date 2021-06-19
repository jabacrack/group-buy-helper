namespace GroupBuyHelper.Dtos
{
    public record ProductListInfo
    {
        public ProductListInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; init; }
        public string Name { get; init; }
    }
}