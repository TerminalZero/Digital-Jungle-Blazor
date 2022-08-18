namespace Digital_Jungle_Blazor.Services.SqlConnections;

public class QueryingConnection {
    public IConfiguration _configuration;
    public MySqlConnector.MySqlConnection Get()
    {
        return new MySqlConnector.MySqlConnection(_configuration["ConnectionStrings:Querying"]);
    }
    
    public QueryingConnection(IConfiguration configuration) => _configuration = configuration;
}