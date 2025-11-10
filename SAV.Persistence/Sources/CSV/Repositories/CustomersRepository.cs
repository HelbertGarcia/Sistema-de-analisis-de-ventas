using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class CustomersRepository : ICustomersRepository
    {
        public Task<List<Customers>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
