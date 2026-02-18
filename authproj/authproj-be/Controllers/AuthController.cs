using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace authproj_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;
  public AuthController(IAuthService authService) => _authService = authService;

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
  {
    try
    {
      var tokens = await _authService.RegisterAsync(request);
      SetTokensInCookies(tokens.AccessToken, tokens.RefreshToken);
      return Ok(new { message = "Registration was successful" });
    }
    catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request)
  {
    try
    {
      var tokens = await _authService.LoginAsync(request);
      SetTokensInCookies(tokens.AccessToken, tokens.RefreshToken);
      return Ok(new { message = "Login was successful" });
    }
    catch (Exception ex) { return Unauthorized(new { message = ex.Message }); }
  }

  [HttpPost("refresh")]
  public async Task<IActionResult> Refresh()
  {
    try
    {
      var refreshToken = Request.Cookies["refreshToken"];
      if (string.IsNullOrEmpty(refreshToken)) return Unauthorized(new { message = "Missing Refresh Token" });

      var tokens = await _authService.RefreshTokensAsync(refreshToken);
      SetTokensInCookies(tokens.AccessToken, tokens.RefreshToken);
      return Ok(new { message = "Tokens were renewed" });
    }
    catch (Exception ex) { return Unauthorized(new { message = ex.Message }); }
  }

  [HttpPost("logout")]
  public IActionResult Logout()
  {
    Response.Cookies.Delete("accessToken");
    Response.Cookies.Delete("refreshToken");
    return Ok(new { message = "Logout was successful" });
  }

  // Protected endpoint for testing Frontend
  [Authorize]
  [HttpGet("me")]
  public IActionResult GetMe()
  {
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    return Ok(new { email });
  }

  private void SetTokensInCookies(string accessToken, string refreshToken)
  {
    var cookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Secure = false, // For local = true, Production = Must be true
      SameSite = SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddMinutes(1) // Short time for testing refresh tokens
    };

    var refreshCookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(7)
    };

    Response.Cookies.Append("accessToken", accessToken, cookieOptions);
    Response.Cookies.Append("refreshToken", refreshToken, refreshCookieOptions);
  }
}
