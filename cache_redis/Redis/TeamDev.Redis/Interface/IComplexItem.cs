using System;
using System.Collections.Generic;

using System.Text;

namespace TeamDevRedis.Interface
{
  public interface IComplexItem
  {
    string KeyName { get; }
    RedisDataAccessProvider Provider { get; }
  }
}
