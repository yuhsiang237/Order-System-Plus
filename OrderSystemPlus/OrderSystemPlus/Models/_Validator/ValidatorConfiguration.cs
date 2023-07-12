using FluentValidation;

using OrderSystemPlus.Models.BusinessActor;

public static class ValidatorConfiguration
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<IValidator<ReqCreateUser>, ReqCreateUserValidator>()
            .AddTransient<IValidator<ReqSignInUser>, ReqSignInUserValidator>()
            .AddTransient<IValidator<ReqUpdateUser>, ReqUpdateUserValidator>()
            .AddTransient<IValidator<ReqDeleteUser>, ReqDeleteUserValidator>();
    }
}
