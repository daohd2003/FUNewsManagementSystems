using Microsoft.AspNetCore.Authentication.Cookies;
using MyMvcApp.Handlers;
using Services;
using System.Net.Http.Headers;

namespace MyMvcApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Thêm HttpContextAccessor trước khi đăng ký các handler
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<JwtTokenHandler>();

            // Đăng ký các HttpClient với base address và handler
            var apiBaseAddress = "https://localhost:7185/api/";

            builder.Services.AddHttpClient<ICategoryService, CategoryService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient<INewsArticleService, NewsArticleService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient<ITagService, TagService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient<ISystemAccountService, SystemAccountService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient("ODataAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7145");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Cấu hình Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn cookie
                });

            builder.Services.AddAuthorization();

            // Cấu hình Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Nên bật trong production
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Thứ tự middleware QUAN TRỌNG:
            app.UseSession();       // Session phải được bật trước Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=NewsArticle}/{action=Index}/{id?}");

            app.Run();
        }
    }
}