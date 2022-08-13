namespace Digital_Jungle_Blazor.Data;

using MySqlConnector;

public class UserService {
    MySqlConnection _connection { get; set; }
    public UserService(MySqlConnection connection) {
        _connection = connection;
    }

    public async Task<User> GetUserById(int id) {
        await _connection.OpenAsync();

        using var command = new MySqlCommand($"SELECT * FROM Users WHERE Id = {id};", _connection);
        using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        
        User userInfo = new() {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            JoinDate = reader.GetDateTime(2)
        };

        return userInfo;
    }
}