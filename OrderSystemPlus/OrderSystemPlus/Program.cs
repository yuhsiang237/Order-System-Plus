using FluentValidation.AspNetCore;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Utils.JwtHelper;

var builder = WebApplication.CreateBuilder(args);

ValidatorConfiguration.Configure(builder);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add auto fluent validation
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

/// <summary>
/// Add dependency injection Handler
/// </summary>
void AddHandler()
{
    builder.Services
        .AddSingleton<IUserManageHandler, UserManageHandler>()
        .AddSingleton<IProductManageHandler, ProductManageHandler>()
        .AddSingleton<IProductInventoryManageHandler, ProductInventoryManageHandler>()
        .AddSingleton<IProductTypeManageHandler, ProductTypeManageHandler>()
        .AddSingleton<IShipmentOrderManageHandler, ShipmentOrderManageHandler>(); ;
}


/// <summary>
/// Add dependency injection Repository
/// </summary>
void AddRepository()
{
    builder.Services
          .AddSingleton<IUserRepository, UserRepository>()
          .AddSingleton<IProductRepository, ProductRepository>()
          .AddSingleton<IProductInventoryRepository, ProductInventoryRepository>()
          .AddSingleton<IProductTypeRepository, ProductTypeRepository>()
          .AddSingleton<IShipmentOrderRepository, ShipmentOrderRepository>()
          .AddSingleton<IProductTypeRelationshipRepository, ProductTypeRelationshipRepository>();

}

// Custom DI
AddRepository();
AddHandler();

// Add JWT
builder.Services.AddSingleton<IJwtHelper, JwtHelper>();


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
