using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces;

public interface IJwtProvider
{
  string GenerateAccessToken(User user);
  string GenerateRefreshToken();
}
