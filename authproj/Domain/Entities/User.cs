using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class User
{
  public UserId Id { get; private set; }
  public string Email { get; private set; }
  public string PasswordHash { get; private set; }
  public string? RefreshToken { get; private set; }
  public DateTime? RefreshTokenExpiryTime { get; private set; }

  private User() { }

  private User(
    UserId id,
    string email,
    string passwordHash)
  {
    Id = id;
    Email = email;
    PasswordHash = passwordHash;
  }

  public static User Create(
    string email,
    string passwordHash)
  {
    return new User(
      UserId.New(),
      email,
      passwordHash);
  }

  public void UpdateRefreshToken(string token, DateTime expiryTime)
  {
    RefreshToken = token;
    RefreshTokenExpiryTime = expiryTime;
  }
}
