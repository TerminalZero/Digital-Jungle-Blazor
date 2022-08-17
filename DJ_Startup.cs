using Microsoft.AspNetCore.Authentication.Cookies;
using Digital_Jungle_Blazor.Services.SqlConnections;
using Digital_Jungle_Blazor.Services.SqlQuerying;

using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Digital_Jungle_Blazor;

public class DJ_Startup
{
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

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KestrelServerOptions>(config => {
            config.ListenAnyIP(443, configure => {
                configure.UseHttps(
                    configuration["Kestrel:Certificates:djcert:KeyPath"],
                    configuration["Kestrel:Certificates:djcert:Password"]
                );
            });
        });

        services.AddRazorPages();
        services.AddServerSideBlazor();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });

        services.AddSingleton<ExampleData.WeatherForecastService>();

        services.AddSingleton<UserInfoService>();
        services.AddTransient<QueryingConnection>();
        services.AddTransient<ValidatingConnection>();

    }
}
