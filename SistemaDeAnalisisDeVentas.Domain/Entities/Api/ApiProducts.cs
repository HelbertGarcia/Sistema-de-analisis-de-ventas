namespace SAV.Domain.Entities.Api
{
    public class ApiProducts
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
    }
}
