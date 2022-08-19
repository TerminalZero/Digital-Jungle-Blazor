namespace Digital_Jungle_Blazor.Services.QueryingService;

using Services.SqlConnections;

public class UserInfoService {
    MySqlConnector.MySqlConnection _qconnection { get; set; }
    MySqlConnector.MySqlConnection _vconnection { get; set; }
    
    public UserInfoService(QueryingConnection qconnection, ValidatingConnection vconnection) {
        _qconnection = qconnection.Get();
        _vconnection = vconnection.Get();
        _qconnection.Open();
        _vconnection.Open();
    }

    public Task<Data.UserInfo> GetUserById(int id)
        => Task.FromResult(GetUsers(id - 1, id).Result.First());
    
    public async Task<List<Data.UserInfo>> GetUsers(int LIMIT_start = 0, int LIMIT_end = 1000) {
        try {
            await _qconnection.OpenAsync();
        } catch {}
        
        using var command = new MySqlConnector.MySqlCommand($"SELECT * FROM UserInfo LIMIT { LIMIT_start }, { LIMIT_end };", _qconnection);
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

        _qconnection.Close();

        return userInfos;
    }

    public async void PushUserInfo(Data.UserInfo userInfo) {
        try {
            await _vconnection.OpenAsync();
        } catch {}
        var command = new MySqlConnector.MySqlCommand($"INSERT INTO UserInfo(Name)" +
            $"VALUE(\"{userInfo.Name}\");"
        , _vconnection);

        using var writer = command.ExecuteNonQueryAsync();
        writer.Wait();
        System.Console.WriteLine("2");

    }
}