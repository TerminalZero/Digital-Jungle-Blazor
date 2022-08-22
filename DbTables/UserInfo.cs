using System.ComponentModel.DataAnnotations;

namespace Digital_Jungle_Blazor.DbTables;

public class UserInfo {
    public int Id { get; set; }
    [MinLength(4)]
    public string Name { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public string Password { get; set; } = string.Empty;

}