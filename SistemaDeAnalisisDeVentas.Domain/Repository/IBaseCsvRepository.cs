
namespace SAV.Domain.Repository
{
    public interface IBaseCsvRepository<TClass>
    {
        Task<List<TClass>> GetAll();
    }
}
