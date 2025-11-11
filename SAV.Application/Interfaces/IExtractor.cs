
namespace SAV.Application.Interfaces
{
    public interface IExtractor<TEntity> where TEntity : class
    {
        string SourceName { get; }
        Task<IEnumerable<TEntity>> ExtractAsync();
    }
}
