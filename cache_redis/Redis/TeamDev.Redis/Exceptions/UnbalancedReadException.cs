using System;
using System.Collections.Generic;

using System.Text;

namespace TeamDevRedis
{
  public class UnbalancedReadException : ApplicationException
  {
    public UnbalancedReadException()
      : base()
    { }

    public UnbalancedReadException(string message)
      : base(message)
    { }
  }
}
