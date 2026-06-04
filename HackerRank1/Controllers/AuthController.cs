using HackerRank1.DTO;
using HackerRank1.Entities;
using HackerRank1.Helpers;
using HackerRank1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackerRank1.Controllers;

public record UserCredential(string Email, string Password);
public record TokenResponse(string Token);

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService authenticationService;
    private readonly JwtSettings jwtSettings;

    public AuthController(
        IAuthenticationService authenticationService,
        JwtSettings jwtSettings)
    {
        this.authenticationService = authenticationService;
        this.jwtSettings = jwtSettings;
    }

    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserCredential user)
    {
        var validUser = await authenticationService.AuthenticateAsync(
            user.Email,
            user.Password
        );

        if (validUser is null)
        {
            return Unauthorized();
        }

        var token = TokenGenerator.GenerateToken(validUser, jwtSettings);

        return Ok(new TokenResponse(token));
    }
}
