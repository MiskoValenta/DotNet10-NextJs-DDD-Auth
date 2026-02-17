using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects;

public readonly record struct UserId(Guid Value)
{
  public static UserId New() => new(Guid.NewGuid());
}
