using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SysLog.Exceptions;
using SysLog.UI.Data;

namespace SysLog.Listeners
{

  internal class UdpListener : Listener
  {
    private static string descriiption = "UDP Listener";
    private UdpClient client;
    private Action<SyslogIpModel> dataCallback;
    public ushort Port{ get; private set; }
    public override void Dispose()
    {
      dataCallback = null;
      if (client != null)
      {
        client.Close();
        client.Dispose();
      }

      GC.SuppressFinalize(this);
    }
    //assign empty lambda to avoid null checks
    public Action<SyslogIpModel> OnRecieve { set{ dataCallback = value == null?(SyslogIpModel val)=> { }:value; } }
    override public string GetDescription()
    {
      return  $"{descriiption} listeing in port {Port}";
    }


    override public void CheckForMessages()
    {
      try
      {
        if (client.Available > 0)
        {
          IPEndPoint ip = new IPEndPoint(IPAddress.Any, Port);
          byte[]? bytes = client.Receive(ref ip);
          string resultData = Encoding.UTF8.GetString(bytes);
          if (resultData == "")
          {
            return;
          }
          dataCallback(new SyslogIpModel(ip,resultData, 2));
        }
      }
      catch (Exception ex) { Console.WriteLine(ex); }
    }
    public UdpListener(Action<SyslogIpModel> callback,ushort port = 514)
    {
      this.Port = port;
      this.OnRecieve = callback;
      client = new UdpClient(port);
    }
  ~UdpListener()
    {

      Dispose();
    }
  }
}
