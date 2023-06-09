using FluentValidation;
using FluentValidation.AspNetCore;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.BusinessActor.Queries;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.DataAccessor.Commands;
using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Commands;
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
        .AddSingleton<ICommandHandler<ReqCreateUser>, UserManageCommandHandler>()
        .AddSingleton<ICommandHandler<ReqSignInUser, RspSignInUser>, UserManageCommandHandler>();

    builder.Services
        .AddSingleton<UserManageCommandHandler, UserManageCommandHandler>()
        .AddSingleton<UserManageCommandHandler, UserManageCommandHandler>();

    builder.Services
          .AddSingleton<IUserManageQueryHandler, UserManageQueryHandler>();
}


/// <summary>
/// Add dependency injection Query/Command
/// </summary>
void AddQueryAndCommand()
{
    // command
    builder.Services
    .AddSingleton<IInsertCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IUpdateCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IInsertCommand<IEnumerable<ProductTypeCommandModel>>, ProductTypeCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<ProductTypeCommandModel>>, ProductTypeCommand>()
    .AddSingleton<IUpdateCommand<IEnumerable<ProductTypeCommandModel>>, ProductTypeCommand>()
    .AddSingleton<IInsertCommand<IEnumerable<ProductProductTypeRelationshipCommandModel>>, ProductProductTypeRelationshipCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<ProductProductTypeRelationshipCommandModel>>, ProductProductTypeRelationshipCommand>();

    // query
    builder.Services
          .AddSingleton<IUserQuery, UserQuery>()
          .AddSingleton<IProductTypeQuery, ProductTypeQuery>()
          .AddSingleton<IProductRepository, ProductRepository>()
          .AddSingleton<IProductProductTypeRelationshipQuery, ProductProductTypeRelationshipQuery>();
}

// Custom DI
AddValidactor();
AddQueryAndCommand();
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
