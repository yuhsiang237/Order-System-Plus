using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.DataAccessor.Commands;
using OrderSystemPlus.Models.DataAccessor.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add custom
builder.Services.AddSingleton<IInsertCommand<IEnumerable<UserCommandModel>>, UserCommand>();
builder.Services.AddSingleton<IDeleteCommand<IEnumerable<UserCommandModel>>, UserCommand>();
builder.Services.AddSingleton<IUpdateCommand<IEnumerable<UserCommandModel>>, UserCommand>();


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
