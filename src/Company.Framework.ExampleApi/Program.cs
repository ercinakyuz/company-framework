using System.Text.Json.Serialization;
using Company.Framework.ExampleApi.Bus.Extensions;
using Company.Framework.ExampleApi.Data.Extensions;
using Company.Framework.ExampleApi.Domain.Extensions;
using Company.Framework.ExampleApi.Http.Extensions;
using Company.Framework.Mediator.Extensions;
using CorrelationId;
using CorrelationId.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var serviceCollection = builder.Services;
serviceCollection.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
serviceCollection.AddEndpointsApiExplorer();
serviceCollection.AddSwaggerGen();
serviceCollection.AddDefaultCorrelationId();
serviceCollection.AddMediator();
serviceCollection.AddDomainComponents();
serviceCollection.AddDataComponents(configuration);
serviceCollection.AddBusComponents();
serviceCollection.AddHttpClients();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//await app.UseConsumersAsync();

app.UseCorrelationId();

app.UseAuthorization();

app.MapControllers();

app.Run();
