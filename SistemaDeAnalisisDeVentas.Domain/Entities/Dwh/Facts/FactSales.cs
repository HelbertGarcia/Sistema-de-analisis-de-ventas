using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAV.Domain.Entities.Dwh.Facts
{
    [Table("FactSales", Schema = "Fact")]
    public class FactSales
    {
        [Key]
        public int OrderKey { get; set; }

        public int DateKey { get; set; }

        public int CustomerKey { get; set; }

        public int ProductKey { get; set; }

        public int StatusKey { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
