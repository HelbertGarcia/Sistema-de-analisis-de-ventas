
namespace SAV.Domain.Entities.Db
{
    public class DbOrders
    {
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        public string? Status { get; set; }
    }
}
