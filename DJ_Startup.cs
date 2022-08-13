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
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(configure => {
            configure.MapBlazorHub();
            configure.MapFallbackToPage("/_Host");
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<Data.WeatherForecastService>();
        
        services.AddTransient<MySqlConnection>(_ => new MySqlConnection(_configuration["ConnectionStrings:Default"]));
        services.AddSingleton<Data.UserInfoService>();
    }
}
