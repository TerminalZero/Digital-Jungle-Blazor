namespace Digital_Jungle_Blazor.Data;

using MySqlConnector;

public class UserInfoService {
    MySqlConnection _connection { get; set; }
    public UserInfoService(MySqlConnection connection) {
        _connection = connection;
    }

    public async Task<UserInfo> GetUserById(int id) {
        await _connection.OpenAsync();

        using var command = new MySqlCommand($"SELECT * FROM UserInfo WHERE Id = {id};", _connection);
        using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        
        UserInfo userInfo = new() {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            JoinDate = reader.GetDateTime(2)
        };

        return userInfo;
    }
}