using SAV.Domain.Entities.Csv;
using SAV.Domain.Repository;

namespace SAV.Application.Repositories.Csv
{
    public interface IOrderDetailsRepository: IBaseCsvRepository<OrderDetails>
    {
    }
}
