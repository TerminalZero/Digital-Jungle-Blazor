using Digital_Jungle_Blazor.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    UserInfoService _userInfoService;
    public AuthController(UserInfoService userInfoService) {
        _userInfoService = userInfoService;
    }

    [HttpPost]
    public async Task<ActionResult> Login([FromForm] string name, [FromForm] string password)
    {
        var validatedUserInfo = await _userInfoService.ValidateUserInfo(name, password);
        
        if (validatedUserInfo != null) {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, validatedUserInfo.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, name)
            }, "auth");
            ClaimsPrincipal claims = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claims);

        }
        return Redirect("/");
    }
}