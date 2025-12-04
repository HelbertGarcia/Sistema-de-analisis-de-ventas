namespace SAV.WKS
{
    using Microsoft.EntityFrameworkCore;
    using SAV.Application.Interfaces;
    using SAV.Application.Services;
    using SAV.Domain.Entities.Api;
    using SAV.Domain.Entities.Csv;
    using SAV.Domain.Entities.Db;
    using SAV.Persistence.Sources.Api.Repositories;
    using SAV.Persistence.Sources.CSV.Repositories;
    using SAV.Persistence.Sources.Db;
    using SAV.Persistence.Sources.Db.Repositories;
    using SAV.Application.Repositories.Dwh; 
    using SAV.Persistence.Destination;     
    using SAV.Persistence.Destination.Repositories; 

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddHttpClient();

            //Servicios de extracción
            builder.Services.AddScoped<IExtractor<CsvProducts>, ProductsRepository>();
            builder.Services.AddScoped<IExtractor<CsvCustomers>, CustomersRepository>();
            builder.Services.AddScoped<IExtractor<CsvSales>, VentasCsvRepository>();

            builder.Services.AddScoped<IExtractor<ApiProducts>, ApiProductRepository>();
            builder.Services.AddScoped<IExtractor<ApiCustomers>, ApiCustomerRepository>(); 

            builder.Services.AddDbContextFactory<SourceDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("SourceDatabase");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IExtractor<DbSales>, DbSalesRepository>();

            //Servicios de transformación

            builder.Services.AddDbContext<DwhDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DestinationDatabase");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IDimProductRepository, DimProductRepository>();
            builder.Services.AddScoped<IDimCustomersRepository, DimCustomersRepository>();
            builder.Services.AddScoped<IDimDateRepository, DimDateRepository>();
            builder.Services.AddScoped<IDimStatusRepository, DimStatusRepository>();
            builder.Services.AddScoped<IFactSalesRepository, FactSalesRepository>();

            //REGISTRO DEL SERVICIO ETL 

            builder.Services.AddScoped<IEtlService, EtlService>();

            var host = builder.Build();
            host.Run();
        }
    }
}