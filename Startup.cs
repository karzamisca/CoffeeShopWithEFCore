using CoffeeShop.Data;
using CoffeeShop.Services;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Register the CoffeeShopContext with SQL Server
        services.AddDbContext<CoffeeShopContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Register services
        services.AddScoped<ProductService>(); // Register ProductService
        services.AddScoped<CartService>(); // Register CartService

        // Add session services
        services.AddSession();

        // Add controllers with views
        services.AddControllersWithViews();

        // Add HttpContextAccessor for session handling in CartService
        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseSession(); // Use session middleware

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Store}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "admin",
                pattern: "Admin/{controller=Products}/{action=Index}/{id?}");
        });
    }
}
