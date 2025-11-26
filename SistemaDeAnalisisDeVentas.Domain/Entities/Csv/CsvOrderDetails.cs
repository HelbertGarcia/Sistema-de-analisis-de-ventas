
namespace SAV.Domain.Entities.Csv
{
    public class CsvOrderDetails
    {
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
