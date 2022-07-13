using Company.Framework.Domain.Model.Aggregate.OfWork.Extensions;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using Company.Framework.Mediator.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceCollection = builder.Services;
serviceCollection.AddControllers();
serviceCollection.AddEndpointsApiExplorer();
serviceCollection.AddSwaggerGen();
serviceCollection.AddMediator();
serviceCollection.AddAggregateOfWork<IActionOfWork, ActionOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
