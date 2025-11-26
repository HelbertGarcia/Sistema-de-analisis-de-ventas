namespace SAV.Application.Interfaces
{
    public interface IEtlService
    {
        Task RunEtlProcessAsync(CancellationToken cancellationToken);
    }
}
