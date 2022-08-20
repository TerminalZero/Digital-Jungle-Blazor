namespace Digital_Jungle_Blazor.SqlConnections;

public class MasterConnection {
    public IConfiguration _configuration;
    public MySqlConnector.MySqlConnection Get()
    {
        return new MySqlConnector.MySqlConnection(_configuration["ConnectionStrings:Master"]);
    }
    
    public MasterConnection(IConfiguration configuration) => _configuration = configuration;
}