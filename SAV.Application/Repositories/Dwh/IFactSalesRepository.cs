using SAV.Domain.Entities.Dwh.Facts;
using SAV.Domain.Repository;

namespace SAV.Application.Repositories.Dwh
{
    // Cambiar 'class' por 'interface' y heredar del base
    public interface IFactSalesRepository : IBaseDwhRepository<FactSales>
    {
    }
}