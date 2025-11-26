using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Domain.Entities.Csv;
using SAV.Persistence.Sources.CSV.Base;
using SAV.Application.Interfaces;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class VentasCsvRepository : BaseCsvRepository, IExtractor<CsvSales>
    {
        private readonly ILogger<VentasCsvRepository> _logger;
        private readonly string _ordersFilePath;
        private readonly string _orderDetailsFilePath;

        public string SourceName => "CsvSales";

        public VentasCsvRepository(IConfiguration configuration, ILogger<VentasCsvRepository> logger)
        {
            _logger = logger;
            _ordersFilePath = configuration.GetSection("CsvFilePaths:Orders").Value ?? string.Empty;
            _orderDetailsFilePath = configuration.GetSection("CsvFilePaths:OrderDetails").Value ?? string.Empty;
        }

        public async Task<IEnumerable<CsvSales>> ExtractAsync()
        {
            _logger.LogInformation("Extracting data from {Source}", SourceName);
            return await GetVentasUnificadasAsync();
        }

        private async Task<List<CsvSales>> GetVentasUnificadasAsync()
        {
            _logger.LogInformation("Starting CSV extraction for Orders and OrderDetails.");

            var ordersList = await ReadCsvFileAsync<CsvOrders>(_ordersFilePath, _logger);
            var detailsList = await ReadCsvFileAsync<CsvOrderDetails>(_orderDetailsFilePath, _logger);

            if (!ordersList.Any() || !detailsList.Any())
            {
                _logger.LogWarning("Orders or OrderDetails list is empty. No join can be performed.");
                return new List<CsvSales>();
            }

            _logger.LogInformation("Joining CSV data...");
            var ordersDictionary = ordersList.ToDictionary(o => o.OrderID);
            var ventasUnificadas = new List<CsvSales>();

            foreach (var detail in detailsList)
            {
                if (ordersDictionary.TryGetValue(detail.OrderID, out var order))
                {
                    ventasUnificadas.Add(new CsvSales
                    {
                        OrderID = order.OrderID,
                        CustomerID = order.CustomerID,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        ProductID = detail.ProductID,
                        Quantity = detail.Quantity,
                        Price = detail.TotalPrice
                    });
                }
            }
            return ventasUnificadas;
        }
    }
}