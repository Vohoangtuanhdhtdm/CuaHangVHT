using AspNetCoreHero.ToastNotification;
using CuaHangVHT.Data;
using CuaHangVHT.Helper;
using CuaHangVHT.Hubs;
using CuaHangVHT.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add services to the container.

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;           
    config.IsDismissable = true;            
    config.Position = NotyfPosition.BottomRight; 
});

builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<TuanStoreContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("TuanStore"));
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSignalR();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/TaiKhoan/DangNhap";
    options.AccessDeniedPath = "/AccessDenied";
   // options.ReturnUrlParameter = "CustomUrl"; // Ðoi tên thành "CustomUrl"
    // expire = 14 days
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");
app.Run();
