using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.Data
{
  internal class SyslogIpModel
  {
    public IPEndPoint ip;
    public string msg;
    public byte severity;
    public SyslogIpModel(IPEndPoint ip, string msg, byte severity)
    {
      this.ip = ip;
      this.msg = msg;
      this.severity = severity;
    }
    public static explicit operator string (SyslogIpModel obj) 
    {
      return $"{obj.msg}|| from {obj.ip.ToString()}|| severity:{obj.severity} ";
    }
  }
}
