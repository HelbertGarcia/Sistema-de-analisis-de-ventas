using SAV.Application.Repositories.Dwh;
using SAV.Domain.Entities.Dwh.Dimentions;

namespace SAV.Persistence.Destination.Repositories
{
    public class DimDateRepository : BaseDwhRepository<DimDate>, IDimDateRepository
    {
        public DimDateRepository(DwhDbContext context) : base(context)
        {
        }
    }
}