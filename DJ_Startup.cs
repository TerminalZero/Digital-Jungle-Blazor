using Microsoft.AspNetCore.Authentication.Cookies;
using Digital_Jungle_Blazor.Services.SqlConnections;
using Digital_Jungle_Blazor.Services.QueryingService;

using Microsoft.AspNetCore.Server.Kestrel.Core;

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
        services.Configure<KestrelServerOptions>(config => {
            try {
                config.ListenAnyIP(443, configure => {
                    configure.UseHttps(
                        _configuration["Kestrel:Certificates:djcert:KeyPath"],
                        _configuration["Kestrel:Certificates:djcert:Password"]
                    );
                });
            } catch (Exception e) {
                System.Console.WriteLine(e.Message);
            }
        });

        services.AddRazorPages(_ => _.RootDirectory = "/RazorPages");
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
