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

var app = builder.Build();

// Configure the HTTP request pipeline.
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
