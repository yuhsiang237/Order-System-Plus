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
#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
#pragma warning restore CS0618 // Type or member is obsolete
builder.Services
    .AddTransient<IValidator<ReqUserCreate>, ReqUserCreateValidator>()
    .AddTransient<IValidator<ReqUserSignIn>, ReqUserSignInValidator>()
    .AddTransient<IValidator<ReqUserUpdate>, ReqUserUpdateValidator>();

// Add custom
builder.Services
    .AddSingleton<IInsertCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IUpdateCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IInsertCommand<IEnumerable<ProductTypeCommandModel>>, ProductTypeCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<ProductTypeCommandModel>>, ProductTypeCommand>()
    .AddSingleton<IUpdateCommand<IEnumerable<ProductTypeCommandModel>>, ProductTypeCommand>()
    .AddSingleton<IInsertCommand<IEnumerable<ProductCommandModel>>, ProductCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<ProductCommandModel>>, ProductCommand>()
    .AddSingleton<IUpdateCommand<IEnumerable<ProductCommandModel>>, ProductCommand>();

builder.Services
      .AddSingleton<IUserQuery, UserQuery>()
      .AddSingleton<IProductTypeQuery, ProductTypeQuery>()
      .AddSingleton<IProductQuery, ProductQuery>();

builder.Services
    .AddSingleton<ICommandHandler<ReqUserCreate>, UserManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqUserSignIn, RspUserSignIn>, UserManageCommandHandler>()
    .AddSingleton<IUserManageQueryHandler, UserManageQueryHandler>()
    .AddSingleton<ICommandHandler<ReqProductTypeCreate>, ProductManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqProductTypeUpdate>, ProductManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqProductTypeDelete>, ProductManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqProductCreate>, ProductManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqProductUpdate>, ProductManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqProductDelete>, ProductManageCommandHandler>()
    .AddSingleton<IProductManageQueryHandler, ProductManageQueryHandler>();

builder.Services
    .AddSingleton<ProductManageCommandHandler, ProductManageCommandHandler>()
    .AddSingleton<UserManageCommandHandler, UserManageCommandHandler>();

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
