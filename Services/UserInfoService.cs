namespace Digital_Jungle_Blazor.Services;

using SqlConnections;

public class UserInfoService {
    MySqlConnector.MySqlConnection _qconnection { get; set; }
    MySqlConnector.MySqlConnection _mconnection { get; set; }
    
    public UserInfoService(QueryingConnection qconnection, MasterConnection mconnection) {
        _qconnection = qconnection.Get();
        _mconnection = mconnection.Get();
        _qconnection.Open();
        _mconnection.Open();
    }

    public Task<DbTables.UserInfo> GetUserById(int id)
        => Task.FromResult(GetUsers(id - 1, id).Result.First());
    
    public async Task<List<DbTables.UserInfo>> GetUsers(int LIMIT_start = 0, int LIMIT_end = 1000) {
        try {
            await _qconnection.OpenAsync();
        } catch {}
        
        using var command = new MySqlConnector.MySqlCommand($"SELECT * FROM UserInfo LIMIT { LIMIT_start }, { LIMIT_end };", _qconnection);
        using var reader = await command.ExecuteReaderAsync();

        List<DbTables.UserInfo> userInfos = new();
        while (await reader.ReadAsync())
        {
            DbTables.UserInfo userInfo = new() {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                JoinDate = reader.GetDateTime(2)
            };
            userInfos.Add(userInfo);
        }

        _qconnection.Close();

        return userInfos;
    }

    public async void PushUserInfo(DbTables.UserInfo userInfo) {
        try {
            await _mconnection.OpenAsync();
        } catch {}
        var command = new MySqlConnector.MySqlCommand($"INSERT INTO UserInfo(Name)" +
            $"VALUE(\"{userInfo.Name}\");"
        , _mconnection);

        using var writer = command.ExecuteNonQueryAsync();
        writer.Wait();
        System.Console.WriteLine("2");

    }
}