namespace Digital_Jungle_Blazor.SqlConnections;

public class ValidatingConnection {
    public IConfiguration _configuration;
    public MySqlConnector.MySqlConnection Get()
    {
        return new MySqlConnector.MySqlConnection(_configuration["ConnectionStrings:Validating"]);
    }
    
    public ValidatingConnection(IConfiguration configuration) => _configuration = configuration;
}