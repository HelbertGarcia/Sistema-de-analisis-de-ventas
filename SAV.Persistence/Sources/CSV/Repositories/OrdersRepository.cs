using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class OrdersRepository : IOrdersRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrdersRepository> _logger;
        private readonly string _filePath;

        public OrdersRepository(IConfiguration configuration,
                                ILogger<OrdersRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = _configuration.GetSection("CsvFilePaths:Orders").Value ?? string.Empty;
        }
        public async Task<List<Orders>> GetAll()
        {

            _logger.LogInformation("Starting to read Orders CSV file from path: {FilePath}", _filePath);
            List<Orders> ordersList = new();

            if (!File.Exists(_filePath))
            {
                _logger.LogWarning("Orders CSV file not found at path: {FilePath}", _filePath);
                return ordersList;
            }

            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

                await foreach (var order in csv.GetRecordsAsync<Orders>())
                {
                    ordersList.Add(order);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the Orders CSV file.");
            }

            return ordersList;
        }
    }
}
