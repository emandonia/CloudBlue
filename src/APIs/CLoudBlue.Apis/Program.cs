using CloudBlue.BusinessServices.UsersAccounts;
using CloudBlue.Data.Repositories;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CLoudBlue.Apis.AccountAuthentications;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using ApiKeyService = CloudBlue.BusinessServices.UsersAccounts.ApiKeyService;
using IApiKeyService = CloudBlue.Domain.Interfaces.Services.IApiKeyService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

Log.Logger = new LoggerConfiguration().WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Information)
    .CreateLogger();

#region Dependencies

builder.Host.UseSerilog(Log.Logger);
builder.Services.AddScoped<IUsersAuthRepository, UsersAuthRepository>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IUsersAuthService, UsersAuthService>();
builder.Services.AddScoped<IUsersSessionService, UsersSessionService>();

#endregion

builder.Services.AddScoped<ApiKeyAuthenticationHandler>();

builder.Services.AddSwaggerGen(setup =>
{
    setup.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme,
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = ApiKeyAuthenticationOptions.HeaderName,
            Type = SecuritySchemeType.ApiKey
        });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, Id = ApiKeyAuthenticationOptions.DefaultScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication()
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme,
        null);

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