using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class OrdersRepository : IOrdersRepository
    {
        public Task<List<Orders>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
