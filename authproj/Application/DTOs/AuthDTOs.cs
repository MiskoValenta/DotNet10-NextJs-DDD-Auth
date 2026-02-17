using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs;

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);
public record AuthTokens(string AccessToken, string RefreshToken);
