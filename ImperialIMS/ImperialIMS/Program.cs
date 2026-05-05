using ImperialIMS.Data;
using ImperialIMS.Helpers;
using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(PolicyTypes.Role, PolicyValues.Admin));

    options.AddPolicy("OfficerOrAbove", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.HasClaim(PolicyTypes.Role, PolicyValues.Admin) ||
            ctx.User.HasClaim(PolicyTypes.Role, PolicyValues.Manager) ||
            ctx.User.HasClaim(PolicyTypes.Role, PolicyValues.Officer)));
});
builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login"; // Set your specific login page path
    });

//Needed for layout testing
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IRepo<Alert>, AlertRepo>();
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<InventoryItem>, InventoryItemRepo>();
builder.Services.AddScoped<IRepo<Item>, ItemRepo>();
builder.Services.AddScoped<IRepo<ItemCategory>, ItemCategoryRepo>();
builder.Services.AddScoped<IRepo<Manifest>, ManifestRepo>();
builder.Services.AddScoped<IRepo<Shipment>, ShipmentRepo>();
builder.Services.AddScoped<IRepo<StorageFacility>, StorageFacilityRepo>();
builder.Services.AddScoped<IRepo<InventoryHistory>, InventoryHistoryRepo>();

builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<ApplicationUserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<InventoryItemService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ManifestService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<StorageFacilityService>();
builder.Services.AddScoped<ReportService>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login"); 
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;
    try
    {
        ApplicationDbContext dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

        await ApplicationUserSeedData.SeedAdminUserAsync(userManager, configuration);
        await SeedData.SeedAsync(dbContext);
    }
    catch (Exception ex)
    {
        Console.Write(ex.Message);
    }
}
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
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
