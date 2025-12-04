using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAV.Domain.Entities.Dwh.Dimentions
{
    [Table("DimDate", Schema = "Dimension")]
    public class DimDate
    {
        [Key]
        public int DateKey { get; set; }

        public DateTime CompleteDate { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string? MonthName { get; set; }
    }
}
