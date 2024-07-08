using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.Services;
using CSI.Unicard.Domain.Interfaces;
using FluentValidation.AspNetCore;
using CSI.Unicard.Infrastructure.Repositories;
using NLog;
using NLog.Web;
using CSI.Unicard.Application.AutoMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

#region AddService
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IOrders, OrderRepository>();
builder.Services.AddScoped<IProducts, ProductRepository>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IOrderItem, OrderItemRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IUnitOfWork, UniteOfWork>();

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
#endregion

#region Logging
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Error);
builder.Host.UseNLog();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt=>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);
});

builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
