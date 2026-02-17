using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services;

public interface IAuthService
{
  Task<AuthTokens> RegisterAsync(RegisterRequest request);
  Task<AuthTokens> LoginAsync(LoginRequest request);
  Task<AuthTokens> RefreshTokensAsync(string refreshToken);
}
