using Infrastructure.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// ⬇️ Register all Infrastructure & external services
builder.Services.AddInfrastructureServices(builder.Configuration);

// MVC & Razor setup
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
