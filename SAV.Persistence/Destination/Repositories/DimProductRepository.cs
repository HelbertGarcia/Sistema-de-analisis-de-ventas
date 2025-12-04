using SAV.Application.Repositories.Dwh;
using SAV.Domain.Entities.Dwh.Dimentions; 

namespace SAV.Persistence.Destination.Repositories
{
    public class DimProductRepository : BaseDwhRepository<DimProducts>, IDimProductRepository
    {
        public DimProductRepository(DwhDbContext context) : base(context)
        {
        }
    }
}