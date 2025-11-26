using SAV.Application.Interfaces;
using SAV.Domain.Entities.Api;
using SAV.Domain.Entities.Csv;
using SAV.Domain.Entities.Db;
using Microsoft.Extensions.Logging;

namespace SAV.Application.Services
{
    public class EtlService : IEtlService
    {
        private readonly ILogger<EtlService> _logger;

        private readonly IExtractor<DbSales> _dbSalesExtractor;
        private readonly IExtractor<CsvSales> _csvSalesExtractor;

        private readonly IExtractor<CsvProducts> _csvProductsExtractor;
        private readonly IExtractor<ApiProducts> _apiProductsExtractor;

        private readonly IExtractor<CsvCustomers> _csvCustomersExtractor;
        private readonly IExtractor<ApiCustomers> _apiCustomersExtractor;

        public EtlService(
            ILogger<EtlService> logger,
            IExtractor<DbSales> dbSalesExtractor,
            IExtractor<CsvSales> csvSalesExtractor,
            IExtractor<CsvProducts> csvProductsExtractor,
            IExtractor<ApiProducts> apiProductsExtractor,
            IExtractor<CsvCustomers> csvCustomersExtractor,
            IExtractor<ApiCustomers> apiCustomersExtractor)
        {
            _logger = logger;
            _dbSalesExtractor = dbSalesExtractor;
            _csvSalesExtractor = csvSalesExtractor;
            _csvProductsExtractor = csvProductsExtractor;
            _apiProductsExtractor = apiProductsExtractor;
            _csvCustomersExtractor = csvCustomersExtractor;
            _apiCustomersExtractor = apiCustomersExtractor;
        }

        public async Task RunEtlProcessAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(">>> INICIANDO PROCESO ETL GLOBAL <<<");

            try
            {
                var sqlSales = await _dbSalesExtractor.ExtractAsync();
                var csvSales = await _csvSalesExtractor.ExtractAsync();

                _logger.LogInformation("Ventas extraídas: {SqlCount} (SQL), {CsvCount} (CSV)",
                                       sqlSales.Count(), csvSales.Count());

                var csvProducts = await _csvProductsExtractor.ExtractAsync();
                var apiProducts = await _apiProductsExtractor.ExtractAsync();

                _logger.LogInformation("Productos extraídos: {CsvCount} (CSV), {ApiCount} (API)",
                                       csvProducts.Count(), apiProducts.Count());

                var csvCustomers = await _csvCustomersExtractor.ExtractAsync();
                var apiCustomers = await _apiCustomersExtractor.ExtractAsync();

                _logger.LogInformation("Clientes extraídos: {CsvCount} (CSV), {ApiCount} (API)",
                                       csvCustomers.Count(), apiCustomers.Count());


                _logger.LogInformation("--- Fase de Extracción Completada ---");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FATAL ERROR en el proceso ETL");
            }
        }
    }
}
