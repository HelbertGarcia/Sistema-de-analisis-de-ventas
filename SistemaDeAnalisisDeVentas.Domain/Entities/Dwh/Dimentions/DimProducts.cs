namespace SAV.Domain.Entities.Dwh.Dimentions
{
    public class DimProducts
    {
        public int ProductKey { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? Category { get; set; }

        public decimal Price { get; set; }
    }
}
