using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAV.Application.Interfaces;
using SAV.Domain.Entities.Csv;
using SAV.Persistence.Sources.CSV.Base;

namespace SAV.Persistence.Sources.CSV.Repositories
{
    public sealed class CustomersRepository : BaseCsvRepository, IExtractor<CsvCustomers>
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

        public async Task<IEnumerable<CsvCustomers>> ExtractAsync()
        {
            _logger.LogInformation("Extracting data from {Source} at path: {FilePath}", SourceName, _filePath);

            List<CsvCustomers> customersList = await ReadCsvFileAsync<CsvCustomers>(_filePath, _logger);

            return customersList;
        }
    }
}