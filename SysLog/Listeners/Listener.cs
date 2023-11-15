using SysLog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Listeners
{
  abstract class Listener
  {
    abstract public string GetDescription();
    abstract public void StartListeing();
    abstract public void StopListening();
    abstract public void Listen();
  }
}
