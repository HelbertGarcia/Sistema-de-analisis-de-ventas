using SAV.Application.Interfaces;

namespace SAV.WKS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker ejecutando ciclo ETL a las: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var etlService = scope.ServiceProvider.GetRequiredService<IEtlService>();

                        await etlService.RunEtlProcessAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creando el scope o resolviendo servicios");
                    }
                }

                _logger.LogInformation("Ciclo terminado. Esperando para la siguiente ejecución...");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
