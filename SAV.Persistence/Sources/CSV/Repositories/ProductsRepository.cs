using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Interfaces;
using SAV.Domain.Entities.Csv;
using SAV.Persistence.Sources.CSV.Base;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class ProductsRepository : BaseCsvRepository, IExtractor<Products>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsRepository> _logger;
        private readonly string _filePath;

        public string SourceName => "CsvProducts";

        public ProductsRepository(IConfiguration configuration,
                                  ILogger<ProductsRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = _configuration.GetSection("CsvFilePaths:Products").Value ?? string.Empty;
        }

        public async Task<IEnumerable<Products>> ExtractAsync()
        {
            _logger.LogInformation("Extracting data from {Source} at path: {FilePath}", SourceName, _filePath);

            List<Products> productsList = await ReadCsvFileAsync<Products>(_filePath, _logger);

            return productsList;
        }
    }
}