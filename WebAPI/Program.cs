using Autofac.Extensions.DependencyInjection;
using Autofac;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Autofac.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new AutofacBusinessModule()));

builder.Services.AddDbContext<ContextDb>();

//builder.Services.AddScoped<IProductService, ProductManager>();
//builder.Services.AddScoped<ICategoryService, CategoryManager>();
//builder.Services.AddScoped<IOrderDetailService, OrderDetailManager>();
//builder.Services.AddScoped<IOrderService, OrderManager>();
//builder.Services.AddScoped<IUserService, UserManager>();

//builder.Services.AddScoped<IProductDal, EfProductDal>();
//builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
//builder.Services.AddScoped<IOrderDal, EfOrderDal>();
//builder.Services.AddScoped<IOrderDetailDal, EfOrderDetailDal>();
//builder.Services.AddScoped<IUserDal, EfUserDal>();

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ContextDb>();

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
