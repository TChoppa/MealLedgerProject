using MealLedger.ApplicationDbContext;
using MealLedger.IRepository;
using MealLedger.IServices;
using MealLedger.Repositories;
using MealLedger.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Repository
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
// Service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDashboardRepositorycs, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();        // ✅ changed from MapStaticAssets
app.UseRouting();
app.UseSession();            // ✅ THIS WAS MISSING!
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();