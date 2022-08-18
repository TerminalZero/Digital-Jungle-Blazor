namespace Digital_Jungle_Blazor.Services.QueryingService;

using Services.SqlConnections;

public class UserInfoService {
    MySqlConnector.MySqlConnection _connection { get; set; }
    
    public UserInfoService(QueryingConnection connection) {
        _connection = connection.Get;
    }

    public Task<Data.UserInfo> GetUserById(int id)
        => Task.FromResult(GetUsers(id - 1, id).Result.First());
    
    public async Task<List<Data.UserInfo>> GetUsers(int LIMIT_start = 0, int LIMIT_end = 1000) {
        await _connection.OpenAsync();

        using var command = new MySqlConnector.MySqlCommand($"SELECT * FROM Data.UserInfo LIMIT { LIMIT_start }, { LIMIT_end };", _connection);
        using var reader = await command.ExecuteReaderAsync();

        List<Data.UserInfo> userInfos = new();
        while (await reader.ReadAsync())
        {
            Data.UserInfo userInfo = new() {
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