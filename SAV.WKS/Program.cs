namespace SAV.WKS
{
    using SAV.Application.Interfaces;
    using SAV.Domain.Entities.Csv;
    using SAV.Persistence.Sources.CSV.Repositories;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddScoped<IExtractor<DbProducts>, ProductsRepository>();
            builder.Services.AddScoped<IExtractor<Customers>, CustomersRepository>();
            builder.Services.AddScoped<IExtractor<DbSales>, VentasCsvRepository>();

            var host = builder.Build();
            host.Run();
        }
    }
}