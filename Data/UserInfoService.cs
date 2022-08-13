namespace Digital_Jungle_Blazor.Data;

using MySqlConnector;

public class UserInfoService {
    MySqlConnection _connection { get; set; }
    public UserInfoService(MySqlConnection connection) {
        _connection = connection;
    }

    public Task<UserInfo> GetUserById(int id)
        => Task.FromResult(GetUsers(id - 1, id).Result.First());
    
    public async Task<List<UserInfo>> GetUsers(int LIMIT_start = 0, int LIMIT_end = 1000) {
        await _connection.OpenAsync();

        using var command = new MySqlCommand($"SELECT * FROM UserInfo LIMIT { LIMIT_start }, { LIMIT_end };", _connection);
        using var reader = await command.ExecuteReaderAsync();

        List<UserInfo> userInfos = new();
        while (await reader.ReadAsync())
        {
            UserInfo userInfo = new() {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                JoinDate = reader.GetDateTime(2)
            };
            userInfos.Add(userInfo);
        }

        _connection.Close();

        return userInfos;
    }
}