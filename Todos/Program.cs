using System.Reflection;
using Microsoft.OpenApi.Models;

using Todos.Persistence.Configuration;
using Todos.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var confSection = builder.Configuration.GetSection(SearchConfiguration.ConfigurationKey);
builder.Services.Configure<SearchConfiguration>(confSection);

var searchConfiguration = new SearchConfiguration();
confSection.Bind(searchConfiguration);

builder.Services.AddPersistence(searchConfiguration);

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo() { Title = "Todo API", Version = "v1" });

    // generate the XML docs that'll drive the swagger docs
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);

    config.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
