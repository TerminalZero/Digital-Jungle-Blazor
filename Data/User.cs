namespace Digital_Jungle_Blazor.Data;

using MySqlConnector;

public class User {
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime JoinDate { get; set; }
}