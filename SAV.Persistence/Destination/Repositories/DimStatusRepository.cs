using SAV.Application.Repositories.Dwh;
using SAV.Domain.Entities.Dwh.Dimentions;

namespace SAV.Persistence.Destination.Repositories
{
    public class DimStatusRepository : BaseDwhRepository<DimStatus>, IDimStatusRepository
    {
        public DimStatusRepository(DwhDbContext context) : base(context)
        {
        }
    }
}