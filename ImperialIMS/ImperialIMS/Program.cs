using ImperialIMS.Data;
using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.ApplicationName = "ImperialIMS";

string logPath = builder.Environment.ContentRootPath + "/IMSLogging";

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.Console())
    .WriteTo.Async(a => a.SQLite( //<-- New section
    sqliteDbPath: logPath + @"-logs.db",
    restrictedToMinimumLevel: LogEventLevel.Information,
    storeTimestampInUtc: true
)));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = !builder.Environment.IsDevelopment())
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Needed for layout testing
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();
builder.Services.AddScoped<IRepo<Alert>, AlertRepo>();
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<InventoryItem>, InventoryItemRepo>();
builder.Services.AddScoped<IRepo<Item>, ItemRepo>();
builder.Services.AddScoped<IRepo<Manifest>, ManifestRepo>();
builder.Services.AddScoped<IRepo<Shipment>, ShipmentRepo>();
builder.Services.AddScoped<IRepo<StorageFacility>, StorageFacilityRepo>();

builder.Services.AddScoped<ApplicationUserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<InventoryItemService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ManifestService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<StorageFacilityService>();
builder.Services.AddScoped<ReportService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
