using FluentValidation;
using FluentValidation.AspNetCore;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Utils.JwtHelper;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add auto fluent validation
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

/// <summary>
/// Add dependency Validactor
/// </summary>
void AddValidactor()
{
    builder.Services
        .AddTransient<IValidator<ReqCreateUser>, ReqCreateUserValidator>()
        .AddTransient<IValidator<ReqSignInUser>, ReqSignInUserValidator>()
        .AddTransient<IValidator<ReqUpdateUser>, ReqUpdateUserValidator>();
}

/// <summary>
/// Add dependency injection Handler
/// </summary>
void AddHandler()
{
    builder.Services
        .AddSingleton<IUserManageHandler, UserManageHandler>()
        .AddSingleton<IProductManageHandler, ProductManageHandler>()
        .AddSingleton<IProductTypeManageHandler, ProductTypeManageHandler>(); ;
}


/// <summary>
/// Add dependency injection Repository
/// </summary>
void AddRepository()
{
    builder.Services
          .AddSingleton<IUserRepository, UserRepository>()
          .AddSingleton<IProductRepository, ProductRepository>()
          .AddSingleton<IProductTypeRepository, ProductTypeRepository>();
}

// Custom DI
AddValidactor();
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
