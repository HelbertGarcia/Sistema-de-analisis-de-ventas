
namespace SAV.Domain.Entities.Csv
{
    public class CsvOrders
    {
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        public string? Status { get; set; }
    }
}
