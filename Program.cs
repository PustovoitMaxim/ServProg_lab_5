using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Генерация SVG иконок
SvgGenerator.CreateAllImages(app.Environment);

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    await SeedData.InitializeAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ===== МАРШРУТЫ - ВАЖНЫЙ ПОРЯДОК! =====

// 1. Специфичные маршруты для Ponds
app.MapControllerRoute(
    name: "ponds_posts",
    pattern: "ponds/posts/{tag}",
    defaults: new { controller = "Ponds", action = "Posts" });

app.MapControllerRoute(
    name: "ponds_trending",
    pattern: "ponds/trending",
    defaults: new { controller = "Ponds", action = "Trending" });

app.MapControllerRoute(
    name: "ponds_all",
    pattern: "ponds/all",
    defaults: new { controller = "Ponds", action = "All" });

app.MapControllerRoute(
    name: "ponds_popular",
    pattern: "ponds/popular",
    defaults: new { controller = "Ponds", action = "Popular" });

// 2. Маршрут для профиля
app.MapControllerRoute(
    name: "profile",
    pattern: "profile/{username}",
    defaults: new { controller = "Profile", action = "Index" });

// 3. Маршрут для отдельного поста
app.MapControllerRoute(
    name: "post",
    pattern: "feed/post/{id}",
    defaults: new { controller = "Feed", action = "Post" });

// 4. Стандартный маршрут (ДОЛЖЕН БЫТЬ ПОСЛЕДНИМ!)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feed}/{action=Index}/{id?}");

app.Run();