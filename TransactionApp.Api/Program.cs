using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TransactionApp.Api.Filters;
using TransactionApp.Api.Middleware;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Mapping;
using TransactionApp.Application.Services;
using TransactionApp.Application.Utilities.Caching;
using TransactionApp.Application.Validators;
using TransactionApp.Domain.Interfaces;
using TransactionApp.Infrastructure.Data;
using TransactionApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddSingleton<IMapper>(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<AutoMapperProfile>();
    }, loggerFactory);

    return config.CreateMapper();
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.Filters.Add(new ValidationFilter());
    });
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction API", Version = "v1" });
    c.UseInlineDefinitionsForEnums();
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ICustomCache, CustomMemoryCache>();
builder.Services.AddScoped<ITransactionSummaryService, TransactionSummaryService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
