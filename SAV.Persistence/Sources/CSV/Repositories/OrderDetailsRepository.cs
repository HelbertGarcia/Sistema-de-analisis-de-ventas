using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class OrderDetailsRepository : IOrderDetailsRepository
    {
        public Task<List<OrderDetails>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
