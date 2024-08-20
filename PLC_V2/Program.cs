using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PLC_V2.PLC_ReadFunction;
using PLC_V2.Model;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Modbus TCP Read API",
        Version = "v1",
        Description = "API for reading coil and register data from PLC using Modbus TCP",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});

// Configure the PLCSettings section from appsettings.json
builder.Services.Configure<PLCSettings>(builder.Configuration.GetSection("PLCSettings"));

// Add the ModbusTCPConnectionFactory as a singleton service using the configuration values
builder.Services.AddSingleton<ModbusTCPConnectionFactory>(sp =>
{
    var config = sp.GetRequiredService<IOptions<PLCSettings>>().Value;
    return new ModbusTCPConnectionFactory(config.IpAddress, config.Port, config.ReconnectInterval);
});

// Add the ModbusTCPRead as a singleton service using the same configuration values
builder.Services.AddSingleton<ModbusTCPRead>(sp =>
{
    var config = sp.GetRequiredService<IOptions<PLCSettings>>().Value;
    return new ModbusTCPRead(config.IpAddress, config.Port, config.ReconnectInterval);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modbus TCP Read API V1");
        c.RoutePrefix = "swagger"; // Swagger UI kök dizinde çalýþýr
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
