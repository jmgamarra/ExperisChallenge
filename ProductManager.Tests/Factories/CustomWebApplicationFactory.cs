using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductManager.Application.Interfaces;
using ProductManager.Infrastructure.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace ProductManager.Tests.Factories
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Elimina la configuración previa de IDbConnection
                services.RemoveAll<IDbConnection>();

                // Registra la conexión a la base de datos con la cadena correcta
                services.AddScoped<IDbConnection>(_ =>
                    new SqlConnection("Server=localhost,1433;Database=ProductManager;User Id=sa;Password=Password123$;TrustServerCertificate=True"));

                // Registra el repositorio para las pruebas
                services.AddScoped<IProductRepository, ProductRepository>();
            });
        }
    }
}
