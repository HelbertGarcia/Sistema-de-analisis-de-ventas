using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAV.Domain.Entities.Dwh.Dimentions
{
    [Table("DimProducts", Schema = "Dimension")]
    public class DimProducts
    {
        [Key]
        public int ProductKey { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? Category { get; set; }

        public decimal Price { get; set; }
    }
}
