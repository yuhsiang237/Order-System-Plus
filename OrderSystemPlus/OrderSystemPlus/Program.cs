using System.Text;

using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

using OrderSystemPlus.Exceptions;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Utils.JwtHelper;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ValidatorConfiguration.Configure(builder);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add Bearer token setting
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization"
        });
    // Add Bearer token to all API
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
        };
    }); ;


// Add auto fluent validation
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
    fv.ValidatorOptions.PropertyNameResolver = OrderSystemPlus.Models.CamelCasePropertyNameResolver.ResolvePropertyName;
});


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
           .AddSingleton<IAuthHandler, AuthHandler>()
           .AddSingleton<IReturnShipmentOrderManageHandler, ReturnShipmentOrderManageHandler>();
}
builder.Services
.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always;
});


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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
                      policy =>
                      {
                          policy
                         .WithOrigins(new string[] { "https://localhost:3000", "http://localhost:3000" })
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseExceptionMiddleware();
app.MapControllers();
app.Run();
