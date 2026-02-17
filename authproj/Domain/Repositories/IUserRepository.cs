using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories;

public interface IUserRepository
{
  Task<User?> GetByEmailAsync(string email);
  Task<User?> GetByRefreshTokenAsync(string refreshToken);
  Task AddAsync(User user);
  Task UpdateAsync(User user);
}
