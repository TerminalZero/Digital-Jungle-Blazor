using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;

namespace Digital_Jungle_Blazor;

public class DJ_Startup
{
    IConfiguration _configuration { get; set; }

    public DJ_Startup(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();
        
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(configure => {
            configure.MapBlazorHub();
            configure.MapFallbackToPage("/_Host");
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });

        services.AddSingleton<Data.WeatherForecastService>();
        
        services.AddTransient<MySqlConnection>(_ => new MySqlConnection(_configuration["ConnectionStrings:Default"]));
        services.AddSingleton<Data.Public.UserInfoService>();
    }
}
