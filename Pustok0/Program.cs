using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pustok0.Context;
using Pustok0.Helpers;

namespace Pustok0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<PustokDbContext>(opt => {
                opt.UseSqlServer(builder.Configuration["ConnectionStrings:MsSql"]);
            });
            PathConstants.RootPath = builder.Environment.WebRootPath;
            var app = builder.Build();
            builder.Services.AddScoped<LayaoutService>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}