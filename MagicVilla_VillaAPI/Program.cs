using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/**Entity framework core: 
 *Microsoft.EntityFrameworkCore.SqlServer (this package also contains entity framework core)
 *Microsoft.EntityFrameworkCore.tools
 */

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

/**Serilog:
 * MinimumLevel: We want errors on the log, above that minimum level.
 * File: In a folder called log, a txt file the logs will be available in an day's interval.
 * */

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog(); // Now the API will use serilog instead of the build in logger.

/*
 * ReturnHttpNotAcceptable is making sure that, this api only returns application/json format.
 * it wont't return any response for other formats (e.g: XML)
 * 
 * To add xml support as well, we need to add .AddXmlDataContractSerializerFormatters();
 * For example:
 * builder.Services.AddControllers(option => {
    option.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
*AddXmlDataContractSerializerFormatters() = this will add XML support to our API
 **/

builder.Services.AddControllers(option => {
    option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

