namespace SAV.Domain.Entities.Dwh.Dimentions
{
    public class DimDate
    {
        public int DateKey { get; set; }

        public DateTime CompleteDate { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string? MonthName { get; set; }
    }
}
