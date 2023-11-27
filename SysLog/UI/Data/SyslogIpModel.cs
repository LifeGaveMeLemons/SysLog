using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.Data
{
  /// <summary>
  ///   This model is meant to hold data ona  raw string that has been recieved andf th IP address that it was recieved from.
  /// </summary>
  internal class SyslogIpModel
  {
    public IPEndPoint ip;
    public string msg;

    /// <summary>
    ///   Returns an instance of SyslogIpModel with al fields filled with the corellating values.
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="msg"></param>
    /// <param name="severity"></param>
    public SyslogIpModel(IPEndPoint ip, string msg, byte severity)
    {
      this.ip = ip;
      this.msg = msg;
    }
  }
}
