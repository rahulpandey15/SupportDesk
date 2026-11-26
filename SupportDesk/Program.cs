using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using SupportDesk.Agents;
using SupportDesk.Configuration;
using SupportDesk.Data;
using SupportDesk.Data.Store;
using SupportDesk.Endpoints;
using SupportDesk.Http;
using SupportDesk.Services;
using SupportDesk.Sessions;
using SupportDesk.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<FoundryOptions>()
    .Bind(builder.Configuration.GetSection(FoundryOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<SupportDeskDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<FoundryOptions>>().Value;
    return new AIProjectClient(new Uri(options.ProjectEndpoint), new DefaultAzureCredential());
});

// Agent infrastructure
builder.Services.AddSingleton<IAgentBuilder, FoundryAgentBuilder>();
builder.Services.AddAgentDefinitions(typeof(Program).Assembly); // discovers every IAgentDefinition
builder.Services.AddScoped<IAgentRegistry, AgentRegistry>();
builder.Services.AddScoped<AgentChatService>();

builder.Services.AddScoped<OrderTools>();
builder.Services.AddScoped<IOrderStore,OrderStore>();

builder.Services.AddScoped<ShipmentTools>();
builder.Services.AddScoped<IShipmentStore, ShipmentStore>();    

builder.Services.AddScoped<RefundTools>();

builder.Services.AddScoped<ICustomerContext,CustomerContext>(); 
builder.Services.AddScoped<ISessionStore,SqlSessionStore>();


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

app.MapChatEndpoints();

app.Run();

