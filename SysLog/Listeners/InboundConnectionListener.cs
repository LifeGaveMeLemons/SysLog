using System.Net.Sockets;
using System.Net;
using SysLog.UI.UiElements;

namespace SysLog.Listeners
{
  /// <summary>
  ///   Checks port for inbound tcp connections.
  /// </summary>
  internal class InboundConnectionListener : IDisposable
  {
    private static string _description = "Tcp Listener";

    private TcpListener _listener;
    public ushort Port { get; private set; }

    /// <summary>
    /// Creates a listener thta chacks for new connections.
    /// </summary>
    /// <param name="port">Selects what port the listener is going to listen on.</param>
    public InboundConnectionListener(int port)
    {
      this.Port = (ushort)port;
      _listener = new TcpListener(new IPEndPoint(ListeningIpAddressView.CurrentListeningAddress, port));
      _listener.Start();
    }

    /// <summary>
    /// Checks for new inbound connections on this port.
    /// </summary>
    /// <returns>null if no new connections have been made, a new TcpClient that is fully initialized and ready to go if a new connection is made.</returns>
     public TcpClient? CheckForConnections()
    {
      if (_listener.Pending())
      {
                return _listener.AcceptTcpClient();
      }
      else return null;
    }

    /// <summary>
    ///   Gets the description of the specific listener type.
    /// </summary>
    /// <returns> A very short description of the type of listener.</returns>
    public string GetDesription()
    {
      return $"{_description} listening on port {Port}";
    }
    /// <summary>
    /// changes the listening Ip address of this lsitener
    /// </summary>
    /// <param name="address">address to be changed to</param>
    public void ChangeIp(IPAddress address)
    {
      _listener.Stop();
      _listener.Server.Bind(new IPEndPoint(address,Port));
      _listener.Start();
    }
    /// <summary>
    ///   Release unamanaged resources.
    /// </summary>
    public void Dispose()
    {
      _listener.Stop();
      _listener = null;
    }


  }
}
