using Infrastructure.Extensions;
using FougeraClub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FougeraClubContextConnection") ?? throw new InvalidOperationException("Connection string 'FougeraClubContextConnection' not found.");;

builder.Services.AddDbContext<FougeraClubContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FougeraClubContext>();

// Register services
builder.Services.ConfigureDatabase(builder.Configuration);
//builder.Services.ConfigureIdentity();
builder.Services.ConfigureApplicationServices();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Register middleware
app.ConfigureMiddleware();

// Seed database
await app.SeedDatabaseAsync();

app.Run();
