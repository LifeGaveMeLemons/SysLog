using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SysLog.Listeners
{
  /// <summary>
  ///   Checks port for inbound tcp connections.
  /// </summary>
  internal class InboundConnectionListener : IDisposable
  {
    private static string description = "Tcp Listener";

    TcpListener listener;
    public ushort Port { get; private set; }

    /// <summary>
    /// Creates a listener thta chacks for new connections.
    /// </summary>
    /// <param name="port">Selects what port the listener is going to listen on.</param>
    public InboundConnectionListener(int port)
    {
      this.Port = (ushort)port;
      listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
      listener.Start();
    }

    /// <summary>
    /// Checks for new inbound connections on this port.
    /// </summary>
    /// <returns>null if no new connections have been made, a new TcpClient that is fully initialized and ready to go if a new connection is made.</returns>
     public TcpClient? CheckForConnections()
    {
      if (listener.Pending())
      {
                Console.WriteLine( "pend");
                return listener.AcceptTcpClient();
      }
      else return null;
    }

    /// <summary>
    ///   Gets the description of the specific listener type.
    /// </summary>
    /// <returns> A very short description of the type of listener.</returns>
    public string GetDesription()
    {
      return $"{description} listening on port {Port}";
    }

    /// <summary>
    ///   Release unamanaged resources.
    /// </summary>
    public void Dispose()
    {
      listener.Stop();
      listener = null;
    }


  }
}
