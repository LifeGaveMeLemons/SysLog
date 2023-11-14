using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Exceptions
{
  internal class UDPAlreadyListeningException : Exception
  {
    public UDPAlreadyListeningException() : base() { }

    public override string ToString()
    {
      return "This UDP client is already listening.";
    }
  }
}
