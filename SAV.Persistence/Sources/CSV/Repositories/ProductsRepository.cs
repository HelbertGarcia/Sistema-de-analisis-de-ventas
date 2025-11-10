using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Repositories.Csv;
using SAV.Domain.Entities.Csv;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class ProductsRepository : IProductsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsRepository> _logger;
        private readonly string _filePath;

        public ProductsRepository(IConfiguration configuration,
                                  ILogger<ProductsRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = _configuration.GetSection("CsvFilePaths:Products").Value ?? string.Empty;
        }
        public async Task<List<Products>> GetAll()
        {
            _logger.LogInformation("Starting to read Products CSV file from path: {FilePath}", _filePath);
            List<Products> productsList = new();

            if (!File.Exists(_filePath))
            {
                _logger.LogWarning("Products CSV file not found at path: {FilePath}", _filePath);
                return productsList;
            }

            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

                await foreach (var product in csv.GetRecordsAsync<Products>())
                {
                    productsList.Add(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the Products CSV file.");
            }

            return productsList;
        }
    }
}
