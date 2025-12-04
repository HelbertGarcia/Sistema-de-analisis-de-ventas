using SAV.Application.Repositories.Dwh;
using SAV.Domain.Entities.Dwh;
using SAV.Domain.Entities.Dwh.Dimentions;

namespace SAV.Persistence.Destination.Repositories
{
    public class DimCustomersRepository : BaseDwhRepository<DimCustomers>, IDimCustomersRepository
    {
        public DimCustomersRepository(DwhDbContext context) : base(context)
        {
        }
    }
}