using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IJwtProvider _jwtProvider;

  public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
  {
    _userRepository = userRepository;
    _passwordHasher = passwordHasher;
    _jwtProvider = jwtProvider;
  }

  public async Task<AuthTokens> RegisterAsync(RegisterRequest request)
  {
    if (await _userRepository.GetByEmailAsync(request.Email) != null)
      throw new Exception("User with this email already exists");

    var hash = _passwordHasher.Hash(request.Password);
    var user = User.Create(request.Email, hash); // Using static factory

    var tokens = GenerateTokensAndUpdateUser(user);
    await _userRepository.AddAsync(user);

    return tokens;
  }

  public async Task<AuthTokens> LoginAsync(LoginRequest request)
  {
    var user = await _userRepository.GetByEmailAsync(request.Email)
        ?? throw new Exception("Neplatné přihlašovací údaje.");

    if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
      throw new Exception("Neplatné přihlašovací údaje.");

    var tokens = GenerateTokensAndUpdateUser(user);
    await _userRepository.UpdateAsync(user);

    return tokens;
  }

  public async Task<AuthTokens> RefreshTokensAsync(string refreshToken)
  {
    var user = await _userRepository.GetByRefreshTokenAsync(refreshToken)
        ?? throw new Exception("Invalid refresh token.");

    if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
      throw new Exception("Refresh token expired.");

    var tokens = GenerateTokensAndUpdateUser(user);
    await _userRepository.UpdateAsync(user);

    return tokens;
  }

  private AuthTokens GenerateTokensAndUpdateUser(User user)
  {
    var accessToken = _jwtProvider.GenerateAccessToken(user);
    var refreshToken = _jwtProvider.GenerateRefreshToken();

    user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
    return new AuthTokens(accessToken, refreshToken);
  }
}
