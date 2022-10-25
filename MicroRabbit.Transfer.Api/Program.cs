using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Trransfer.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer microservice v1", Version = "v1" });
});

builder.Services.AddDbContext<TransferDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("TransferDbConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

RegisterServices(builder.Services);

void RegisterServices(IServiceCollection services)
{
    DependencyContainer.RegisterServices(services);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/Swagger/v1/swagger.json", "Transfer Microservice v1");
    });
}

ConfigureEventBus(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//------------------------------------------------------//

void ConfigureEventBus(WebApplication app)
{
    var eventBus = app.Services.GetRequiredService<IEventBus>();
    eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
    
}