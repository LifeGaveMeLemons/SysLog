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

  /// <summary>
  ///   Listens for connections on specified port
  /// </summary>
  internal class UdpListener : Listener
  {
    private static string descriiption = "UDP Listener";
    private UdpClient client;
    private Action<SyslogIpModel> dataCallback;
    public ushort Port{ get; private set; }

    /// <summary>
    ///   Releases unamanaged resources.
    /// </summary>
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
    /// <summary>
    /// assign empty lambda to avoid null checks
    /// </summary>
    public Action<SyslogIpModel> OnRecieve { set{ dataCallback = value == null?(SyslogIpModel val)=> { }:value; } }

    /// <summary>
    ///   Gets the description of the specific listener type.
    /// </summary>
    /// <returns> A very short description of the type of listener.</returns>
    override public string GetDescription()
    {
      return  $"{descriiption} listeing in port {Port}";
    }

    /// <summary>
    ///   Checks the client for new inbound messages.
    /// </summary>
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

    /// <summary>
    ///   creates a instance of UdpListener, sets the callback for recieved messages and listens on the specified port.
    /// </summary>
    /// <param name="callback">Method to be invoked when there are recievedmessages.</param>
    /// <param name="port">Port to listen on.</param>
    public UdpListener(Action<SyslogIpModel> callback,ushort port = 514)
    {
      this.Port = port;
      this.OnRecieve = callback;
      client = new UdpClient(port);
    }

    /// <summary>
    ///   Clear resources
    /// </summary>
  ~UdpListener()
    {

      Dispose();
    }
  }
}
