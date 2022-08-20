namespace Digital_Jungle_Blazor.Services;

using SqlConnections;

public class UserInfoService {
    MySqlConnector.MySqlConnection _qconnection { get; set; }
    MySqlConnector.MySqlConnection _mconnection { get; set; }
    
    public UserInfoService(QueryingConnection qconnection, MasterConnection mconnection) {
        _qconnection = qconnection.Get();
        _mconnection = mconnection.Get();
    }

    public Task<DbTables.UserInfo> GetUserById(int id)
        => Task.FromResult(GetUsers(id - 1, id).Result.First());
    
    public async Task<List<DbTables.UserInfo>> GetUsers(int LIMIT_start = 0, int LIMIT_end = 1000) {
        if (_qconnection.State != System.Data.ConnectionState.Open) {
            await _qconnection.OpenAsync();
        }
        
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

        return userInfos;
    }

    public async void PushUserInfo(DbTables.UserInfo userInfo) {
        if (_mconnection.State != System.Data.ConnectionState.Open) {
            await _mconnection.OpenAsync();
        }

        var command = new MySqlConnector.MySqlCommand(
            $"INSERT INTO UserInfo(Id, Name)" +
            $"SELECT COUNT(Id), \"{userInfo.Name}\" FROM UserInfo LIMIT 1;" +
            $"INSERT INTO UserInfo_Private(Id, Password)" +
            $"SELECT COUNT(Id), \"{userInfo.Password}\" FROM UserInfo LIMIT 1;"
        , _mconnection).ExecuteNonQueryAsync();
    }
}