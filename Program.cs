using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RoleBaseAuthentication.entities;

namespace RoleBaseAuthentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SampledbContext>(option => option.UseMySQL("Server=localhost;Port=3306;userid=root;password=1234;database=sampledb;pooling=true;SSL Mode=None"));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.AccessDeniedPath = "/AccessDenied";
            options.LoginPath = "/Login";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
        });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admins", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Users", policy => policy.RequireRole("User"));
            });
           // builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1); // Adjust the timeout as needed
                //options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
            });
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.Run();
        }
    }
}