using AlzheimerApp.Data;
using AlzheimerApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add services to the container
builder.Services.AddControllersWithViews();

// 2️⃣ Add DbContext for EF Core
builder.Services.AddDbContext<AlzheimerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3️⃣ Add FlaskApiService with HttpClient
builder.Services.AddHttpClient<FlaskApiService>();

// 4️⃣ Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 5️⃣ Optional: Add Razor Pages if needed
builder.Services.AddRazorPages();

var app = builder.Build();

// 6️⃣ Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // session middleware must be before authorization
app.UseAuthorization();

// 7️⃣ Configure MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Optional: Map Razor pages if used
app.MapRazorPages();

app.Run();
