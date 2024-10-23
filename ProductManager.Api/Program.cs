using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProductManager.Api.Config;
using ProductManager.Api.Models;
using ProductManager.Application.Interfaces;
using ProductManager.Application.Services;
using ProductManager.Infrastructure.Repositories;
using System.Data;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Registrar los servicios de FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<AddProductValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<UpdateProductValidator>();
        fv.AutomaticValidationEnabled = true;
    });
// Configurar JWT Authentication
var jwtSettings = new JwtSettings();
builder.Configuration.Bind("Jwt", jwtSettings);

// Registrar JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// Registrar JwtSettings para inyección de dependencias
builder.Services.AddSingleton(jwtSettings);


// Add services to the container.

// Add bd connection
builder.Services.AddScoped<IDbConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("ProductManagerConnection")));

//agregar servicios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IUserSecurityRepository, UserSecurityRepository>();
builder.Services.AddScoped<UserSecurityRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductManager API V1");
    });
}
//inyectar middleware
app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Expose the Program class for testing purposes
public partial class Program { }

