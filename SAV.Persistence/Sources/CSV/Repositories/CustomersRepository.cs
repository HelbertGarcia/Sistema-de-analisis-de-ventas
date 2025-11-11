using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Interfaces;
using SAV.Domain.Entities.Csv;
using SAV.Persistence.Sources.CSV.Base;
using System.Collections.Generic; 
using System.Threading.Tasks; 

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class CustomersRepository : BaseCsvRepository, IExtractor<Customers>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomersRepository> _logger;
        private readonly string _filePath;

        public string SourceName => "CsvCustomers";

        public CustomersRepository(IConfiguration configuration,
                                     ILogger<CustomersRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = _configuration.GetSection("CsvFilePaths:Customers").Value ?? string.Empty;
        }

        public async Task<IEnumerable<Customers>> ExtractAsync()
        {
            _logger.LogInformation("Extracting data from {Source} at path: {FilePath}", SourceName, _filePath);

            List<Customers> customersList = await ReadCsvFileAsync<Customers>(_filePath, _logger);

            return customersList;
        }
    }
}