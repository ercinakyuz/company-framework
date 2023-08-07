using System.Text.Json;
using System.Text.Json.Serialization;
using Company.Framework.Api.Extensions;
using Company.Framework.Api.Localization.Extensions;
using Company.Framework.Core.Serialization.Extensions;
using Company.Framework.Correlation.Extensions;
using Company.Framework.ExampleApi;
using Company.Framework.ExampleApi.Application.Extensions;
using Company.Framework.ExampleApi.Bus.Extensions;
using Company.Framework.ExampleApi.Data.Extensions;
using Company.Framework.ExampleApi.Domain.Extensions;
using Company.Framework.ExampleApi.Http.Extensions;
using Company.Framework.Logging.Extensions;
using Company.Framework.Socket.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var services = builder.Services;

services.AddJsonSerializer();
services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCorrelation();
services.AddApplicationComponents();
services.AddDomainComponents();
services.AddDataComponents(configuration);
services.AddBusComponents();
services.AddHttpClients();
services.AddApiExceptionHandler();
services.AddLocalization<ExampleApiResource>();
services.AddSocket();

var hostBuilder = builder.Host;
hostBuilder.WithSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//await app.UseConsumersAsync();

app.UseApi();

app.UseAuthorization();

app.MapControllers();

app.UseSocket<MyHub>("/hub");


app.Run();