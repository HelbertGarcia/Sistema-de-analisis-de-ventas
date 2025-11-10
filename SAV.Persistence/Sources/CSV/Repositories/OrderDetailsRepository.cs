using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderDetailsRepository> _logger;
        private readonly string _filePath;

        public OrderDetailsRepository(IConfiguration configuration, 
                                      ILogger<OrderDetailsRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = _configuration.GetSection("CsvFilePaths:OrderDetails").Value ?? string.Empty;
        }
        public async Task<List<OrderDetails>> GetAll()
        {
            _logger.LogInformation("Starting to read Order Details CSV file from path: {FilePath}", _filePath);
            List<OrderDetails> orderDetailsList = new();

            if (!File.Exists(_filePath))
            {
                _logger.LogWarning("Order Details CSV file not found at path: {FilePath}", _filePath);
                return orderDetailsList;
            }

            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

                await foreach (var orderDetails in csv.GetRecordsAsync<OrderDetails>())
                {
                    orderDetailsList.Add(orderDetails);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the Order Details CSV file.");
            }

            return orderDetailsList;
        }
    }
}
