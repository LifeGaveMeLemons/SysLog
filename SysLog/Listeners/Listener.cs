using SysLog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Listeners
{
  abstract class Listener: IDisposable
  {
    abstract public void Dispose();
    abstract public string GetDescription();
    abstract public void CheckForMessages();
  }
}
