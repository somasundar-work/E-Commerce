using System.Text.Json.Serialization;
using Application.Mapper;
using Application.ProductsRepository;
using Data.Context;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingDefault;
    });

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddProblemDetails(opts =>
{
    // Control when an exception is included
    opts.IncludeExceptionDetails = (ctx, ex) =>
    {
        // Fetch services from HttpContext.RequestServices
        var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
        return env.IsDevelopment() || env.IsStaging();
    };
});

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("StoreDB"));
});

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "E-Commerce API",
            Description = "E-Commerce using .NET 8 and Angular 18",
        }
    );
});

var app = builder.Build();

app.UseProblemDetails(); // Add the middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.DocumentTitle = "E-Commerce using .NET 8 and Angular 18";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
