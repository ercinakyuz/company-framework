using Company.Framework.ExampleApi.Domain.Extensions;
using Company.Framework.Mediator.Extensions;
using CorrelationId;
using CorrelationId.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceCollection = builder.Services;
serviceCollection.AddControllers();
serviceCollection.AddEndpointsApiExplorer();
serviceCollection.AddSwaggerGen();
serviceCollection.AddDefaultCorrelationId();
serviceCollection.AddMediator();
serviceCollection.AddDomainComponents();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCorrelationId();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
