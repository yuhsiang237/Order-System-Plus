using FluentValidation.AspNetCore;
using OrderSystemPlus.Exceptions;
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
           .AddSingleton<IShipmentOrderManageHandler, ShipmentOrderManageHandler>()
           .AddSingleton<IReturnShipmentOrderManageHandler, ReturnShipmentOrderManageHandler>();
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
         .AddSingleton<IReturnShipmentOrderRepository, ReturnShipmentOrderRepository>()
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

// CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionMiddleware();
app.MapControllers();

app.Run();
