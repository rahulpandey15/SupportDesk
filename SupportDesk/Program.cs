using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SupportDesk.Data;
using SupportDesk.Data.Store;
using SupportDesk.Endpoints;
using SupportDesk.Http;
using SupportDesk.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SupportDeskDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();  


builder.Services.AddScoped<OrderTools>();
builder.Services.AddScoped<IOrderStore,OrderStore>();
builder.Services.AddScoped<ICustomerContext,CustomerContext>(); 


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseMiddleware<CustomerContextMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var orderTools = scope.ServiceProvider.GetRequiredService<OrderTools>();
    app.MapChatEndpoints(orderTools);
}


app.Run();

