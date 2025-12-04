using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAV.Domain.Entities.Dwh.Dimentions
{
    [Table("DimStatus", Schema = "Dimension")]
    public class DimStatus
    {
        [Key]
        public int StatusKey { get; set; }
        
        public string? Status { get; set; }
    }
}
