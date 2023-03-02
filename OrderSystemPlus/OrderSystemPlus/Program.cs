using FluentValidation;
using FluentValidation.AspNetCore;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.BusinessActor.Commands;
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
builder.Services.AddTransient<IValidator<ReqUserManageCreate>, ReqUserManageCreateValidator>();
builder.Services.AddTransient<IValidator<ReqSignInUser>, ReqSignInUserValidator>();

// Add custom
builder.Services
    .AddSingleton<IInsertCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IDeleteCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IUpdateCommand<IEnumerable<UserCommandModel>>, UserCommand>()
    .AddSingleton<IUserQuery, UserQuery>();

builder.Services
    .AddSingleton<ICommandHandler<ReqUserManageCreate>, UserManageCommandHandler>()
    .AddSingleton<ICommandHandler<ReqSignInUser, RspSignInUser>, UserManageCommandHandler>()
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
