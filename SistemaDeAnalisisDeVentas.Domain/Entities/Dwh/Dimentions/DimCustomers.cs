using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAV.Domain.Entities.Dwh.Dimentions
{
    [Table("DimCustomers", Schema = "Dimension")]
    public class DimCustomers
    {
        [Key]
        public int CustomerKey { get; set; }

        public int CustomerId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }
    }
}
