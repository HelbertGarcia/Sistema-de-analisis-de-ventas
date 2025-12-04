using SAV.Application.Repositories.Dwh;
using SAV.Domain.Entities.Dwh;
using SAV.Domain.Entities.Dwh.Facts;

namespace SAV.Persistence.Destination.Repositories
{
    public class FactSalesRepository : BaseDwhRepository<FactSales>, IFactSalesRepository
    {
        public FactSalesRepository(DwhDbContext context) : base(context)
        {
        }
    }
}