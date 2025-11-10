namespace SAV.Domain.Entities.Dwh.Facts
{
    public class FactSales
    {
        public int OrderKey { get; set; }

        public int DateKey { get; set; }

        public int CustomerKey { get; set; }

        public int ProductKey { get; set; }

        public int StatusKey { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
