using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SysLog.Listeners
{
  internal class InboundConnectionListener : IDisposable
  {
    private static string description = "Tcp Listener";

    TcpListener listener;
    public ushort Port { get; private set; }
    public InboundConnectionListener(int port)
    {
      this.Port = (ushort)port;
      listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
      listener.Start();
    }
     public TcpClient? CheckForConnections()
    {
      if (listener.Pending())
      {
                Console.WriteLine( "pend");
                return listener.AcceptTcpClient();
      }
      else return null;
    }
    public string GetDesription()
    {
      return $"{description} listening on port {Port}";
    }
    public void Dispose()
    {
      listener.Stop();
      listener = null;
    }


  }
}
