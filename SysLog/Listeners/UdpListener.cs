using System.Text;
using System.Net;
using System.Net.Sockets;
using SysLog.UI.Data;
using SysLog.UI.UiElements;

namespace SysLog.Listeners
{

  /// <summary>
  ///   Listens for connections on specified port
  /// </summary>
  internal class UdpListener : Listener
  {
    private static string s_descriiption = "UDP Listener";
    private UdpClient _client;
    private Action<SyslogIpModel> _dataCallback;
    public ushort Port{ get; private set; }

    /// <summary>
    ///   Releases unamanaged resources.
    /// </summary>
    public override void Dispose()
    {
      _dataCallback = null;
      if (_client != null)
      {
        _client.Close();
        _client.Dispose();
      }

      GC.SuppressFinalize(this);
    }
    /// <summary>
    /// assign empty lambda to avoid null checks
    /// </summary>
    public Action<SyslogIpModel> OnRecieve { set{ _dataCallback = value == null?(SyslogIpModel val)=> { }:value; } }

    /// <summary>
    ///   Gets the description of the specific listener type.
    /// </summary>
    /// <returns> A very short description of the type of listener.</returns>
    override public string GetDescription()
    {
      return  $"{s_descriiption} listeing in port {Port}";
    }

    /// <summary>
    ///   Checks the client for new inbound messages.
    /// </summary>
    override public void CheckForMessages()
    {
      try
      {
        lock (this)
        {
          if (_client.Available > 0)
          {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, Port);
            byte[]? bytes = _client.Receive(ref ip);
            string resultData = Encoding.UTF8.GetString(bytes);
            if (resultData == "")
            {
              return;
            }
            _dataCallback(new SyslogIpModel(ip, resultData,false));
          }
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
      _client = new UdpClient();
      _client.Client.Bind(new IPEndPoint(ListeningIpAddressView.CurrentListeningAddress, port));
      
    }
    public void ChangeIp(IPAddress address)
    {
      lock (this)
      {
        _client.Close();
        _client.Dispose();
        _client = new UdpClient();
        _client.Client.Bind(new IPEndPoint(address, Port));
      }
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
