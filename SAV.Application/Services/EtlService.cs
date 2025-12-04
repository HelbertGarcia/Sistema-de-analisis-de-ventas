using Microsoft.Extensions.Logging;
using SAV.Application.Interfaces;
using SAV.Application.Repositories.Dwh;
using SAV.Domain.Entities.Api;
using SAV.Domain.Entities.Csv;
using SAV.Domain.Entities.Db;
using SAV.Domain.Entities.Dwh;
using SAV.Domain.Entities.Dwh.Dimentions;

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
        private readonly IDimProductRepository _dimProductRepo;
        private readonly IDimCustomersRepository _dimCustomerRepo;
        private readonly IDimDateRepository _dimDateRepo;
        private readonly IDimStatusRepository _dimStatusRepo;
        private readonly IFactSalesRepository _factSalesRepo;

        public EtlService(
            ILogger<EtlService> logger,
            IExtractor<DbSales> dbSalesExtractor,
            IExtractor<CsvSales> csvSalesExtractor,
            IExtractor<CsvProducts> csvProductsExtractor,
            IExtractor<ApiProducts> apiProductsExtractor,
            IExtractor<CsvCustomers> csvCustomersExtractor,
            IExtractor<ApiCustomers> apiCustomersExtractor,
            IDimProductRepository dimProductRepo,
            IDimCustomersRepository dimCustomerRepo,
            IDimDateRepository dimDateRepo,
            IDimStatusRepository dimStatusRepo,
            IFactSalesRepository factSalesRepo)
        {
            _logger = logger;
            _dbSalesExtractor = dbSalesExtractor;
            _csvSalesExtractor = csvSalesExtractor;
            _csvProductsExtractor = csvProductsExtractor;
            _apiProductsExtractor = apiProductsExtractor;
            _csvCustomersExtractor = csvCustomersExtractor;
            _apiCustomersExtractor = apiCustomersExtractor;
            _dimProductRepo = dimProductRepo;
            _dimCustomerRepo = dimCustomerRepo;
            _dimDateRepo = dimDateRepo;
            _dimStatusRepo = dimStatusRepo;
            _factSalesRepo = factSalesRepo;
        }

        public async Task RunEtlProcessAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(">>> INICIANDO PROCESO ETL: CARGA DE DIMENSIONES <<<");

            try
            {
                // 1. EXTRACCIÓN (Obtener datos crudos)
                var sqlSales = await _dbSalesExtractor.ExtractAsync();
                var csvSales = await _csvSalesExtractor.ExtractAsync();

                // Productos
                var csvProducts = await _csvProductsExtractor.ExtractAsync();
                var apiProducts = await _apiProductsExtractor.ExtractAsync();

                // Clientes
                var csvCustomers = await _csvCustomersExtractor.ExtractAsync();
                var apiCustomers = await _apiCustomersExtractor.ExtractAsync();

                _logger.LogInformation("Datos extraídos en memoria. Iniciando Carga de Dimensiones...");


                // 2. CARGA DE DIMENSIONES (Load Dimensions)

                await LoadDimProducts(csvProducts, apiProducts);

                await LoadDimCustomers(csvCustomers, apiCustomers);

                var allStatuses = sqlSales.Select(s => s.Status)
                                  .Concat(csvSales.Select(s => s.Status));
                await LoadDimStatus(allStatuses);

                var allDates = sqlSales.Select(s => s.OrderDate)
                               .Concat(csvSales.Select(s => s.OrderDate));
                await LoadDimDate(allDates);


                _logger.LogInformation(">>> CARGA DE DIMENSIONES COMPLETADA <<<");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR FATAL durante la carga de dimensiones.");
            }
        }

        private async Task LoadDimProducts(IEnumerable<CsvProducts> csvProds, IEnumerable<ApiProducts> apiProds)
        {
            // 1. Traer IDs existentes para no duplicar (Cache)
            var existingIds = (await _dimProductRepo.GetAll())
                              .Select(p => p.ProductId) // Asumiendo ProductId es int
                              .ToHashSet();

            var newProducts = new List<DimProducts>();

            // 2. Procesar CSV
            var fromCsv = csvProds
                .Where(p => !existingIds.Contains(p.ProductID))
                .Select(p => new DimProducts
                {
                    ProductId = p.ProductID,
                    ProductName = p.ProductName,
                    Category = p.Category,
                    Price = p.Price
                });
            newProducts.AddRange(fromCsv);

            // Actualizamos el cache local para no duplicar si la API trae el mismo ID que el CSV
            foreach (var p in fromCsv) existingIds.Add(p.ProductId);

            // 3. Procesar API
            var fromApi = apiProds
                .Where(p => !existingIds.Contains(p.Id))
                .Select(p => new DimProducts
                {
                    ProductId = p.Id,
                    ProductName = p.Title,
                    Category = p.Category,
                    Price = p.Price
                });
            newProducts.AddRange(fromApi);

            // 4. Guardar en DB
            if (newProducts.Any())
            {
                await _dimProductRepo.SaveAll(newProducts.ToArray());
                _logger.LogInformation("DimProducts: {Count} nuevos registros insertados.", newProducts.Count);
            }
            else
            {
                _logger.LogInformation("DimProducts: No hay nuevos registros.");
            }
        }

        private async Task LoadDimCustomers(IEnumerable<CsvCustomers> csvCusts, IEnumerable<ApiCustomers> apiCusts)
        {
            // 1. Cache de existentes
            var existingIds = (await _dimCustomerRepo.GetAll())
                              .Select(c => c.CustomerId)
                              .ToHashSet();

            var newCustomers = new List<DimCustomers>();

            // 2. Procesar CSV
            var fromCsv = csvCusts
                .Where(c => !existingIds.Contains(c.CustomerID))
                .Select(c => new DimCustomers
                {
                    CustomerId = c.CustomerID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    City = c.City,
                    Country = c.Country
                });
            newCustomers.AddRange(fromCsv);

            foreach (var c in fromCsv) existingIds.Add(c.CustomerId);

            // 3. Procesar API
            var fromApi = apiCusts
                .Where(c => !existingIds.Contains(c.Id))
                .Select(c => new DimCustomers
                {
                    CustomerId = c.Id,
                    FirstName = c.Name, // La API usa 'Name'
                    Email = c.Email,
                    Country = c.Country,
                    City = "Unknown",   // Valor por defecto
                    Phone = "Unknown"   // Valor por defecto
                });
            newCustomers.AddRange(fromApi);

            // 4. Guardar
            if (newCustomers.Any())
            {
                await _dimCustomerRepo.SaveAll(newCustomers.ToArray());
                _logger.LogInformation("DimCustomers: {Count} nuevos registros insertados.", newCustomers.Count);
            }
        }

        private async Task LoadDimStatus(IEnumerable<string?> statuses)
        {
            // 1. Cache existentes
            var existingStatuses = (await _dimStatusRepo.GetAll())
                                   .Select(s => s.Status)
                                   .ToHashSet();

            // 2. Filtrar únicos y nuevos
            var newStatuses = statuses
                .Where(s => !string.IsNullOrEmpty(s)) // Ignorar nulos/vacíos
                .Distinct() // Eliminar duplicados en memoria
                .Where(s => !existingStatuses.Contains(s)) // Filtrar los que ya están en BD
                .Select(s => new DimStatus { Status = s })
                .ToArray();

            // 3. Guardar
            if (newStatuses.Any())
            {
                await _dimStatusRepo.SaveAll(newStatuses);
                _logger.LogInformation("DimStatus: {Count} nuevos registros insertados.", newStatuses.Length);
            }
        }

        private async Task LoadDimDate(IEnumerable<DateTime> dates)
        {
            // 1. Cache existentes (Solo las llaves int)
            var existingDateKeys = (await _dimDateRepo.GetAll())
                                   .Select(d => d.DateKey)
                                   .ToHashSet();

            // 2. Procesar
            var newDates = dates
                .Select(d => d.Date) // Quitar la hora
                .Distinct()
                .Select(d => new
                {
                    DateObj = d,
                    Key = (d.Year * 10000) + (d.Month * 100) + d.Day // Generar Key: 20251125
                })
                .Where(x => !existingDateKeys.Contains(x.Key)) // Filtrar existentes
                .Select(x => new DimDate
                {
                    DateKey = x.Key,
                    CompleteDate = x.DateObj,
                    Year = x.DateObj.Year,
                    Month = x.DateObj.Month,
                    MonthName = x.DateObj.ToString("MMMM")
                })
                .ToArray();

            // 3. Guardar
            if (newDates.Any())
            {
                await _dimDateRepo.SaveAll(newDates);
                _logger.LogInformation("DimDate: {Count} nuevos registros insertados.", newDates.Length);
            }
        }
    }
}