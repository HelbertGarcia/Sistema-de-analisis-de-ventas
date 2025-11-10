using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class CustomersRepository : ICustomersRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomersRepository> _logger;
        private readonly string _filePath;

        public CustomersRepository(IConfiguration configuration, 
                                   ILogger<CustomersRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = _configuration.GetSection("CsvFilePaths:Customers").Value ?? string.Empty;
        }
        public async Task<List<Customers>> GetAll()
        {
            _logger.LogInformation("Starting to read Customers CSV file from path: {FilePath}", _filePath);
            List<Customers> customersList = new();

            if (!File.Exists(_filePath))
            {
                _logger.LogWarning("Customers CSV file not found at path: {FilePath}", _filePath);
                return customersList;
            }

            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

                await foreach (var customer in csv.GetRecordsAsync<Customers>())
                {
                    customersList.Add(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the Customers CSV file.");
            }

            return customersList;
        }
    }
}
