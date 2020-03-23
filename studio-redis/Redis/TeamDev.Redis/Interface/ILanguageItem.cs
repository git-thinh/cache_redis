using System;
using System.Collections.Generic;

using System.Text;

namespace TeamDevRedis.Interface
{
  public interface ILanguageItem
  {
    void Configure(string name, RedisDataAccessProvider provider);
  }
}
