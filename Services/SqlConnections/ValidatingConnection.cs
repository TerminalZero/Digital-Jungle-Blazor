namespace Digital_Jungle_Blazor.Services.SqlConnections;

public class ValidatingConnection {
    public IConfiguration _configuration;
    public MySqlConnector.MySqlConnection Get
    {
        get => new MySqlConnector.MySqlConnection(_configuration["ConnectionStrings:Validating"]);
    }
    
    public ValidatingConnection(IConfiguration configuration) => _configuration = configuration;
}