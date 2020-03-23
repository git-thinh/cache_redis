using System;
using System.Collections.Generic;

using System.Text;
using TeamDevRedis.LanguageItems;
using System.Net.Sockets;

namespace TeamDevRedis
{
  public abstract class DataAccessProvider
  {
    public Configuration Configuration { get; set; }

    public DataAccessProvider()
    {
      Configuration = new LanguageItems.Configuration();
    }

    public abstract Socket Connect();
    public abstract void Close();

  }
}
