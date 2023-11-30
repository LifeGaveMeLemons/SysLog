using SysLog.UI.Data;
using SysLog.UI.UiElements.SettingsView;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SysLog.Listeners
{
  /// <summary>
  ///   Represnets a Tcp connection with a client, removes itself when connection is terminated.
  /// </summary>
  internal class TcpSessionListener : Listener
  {
    private static string s_description = "Tcp Handler";
    private List<string> _allMessages = new List<string>();
    private Action<SyslogIpModel> _callback;
    private Action<TcpSessionListener> _rm;
		private IPEndPoint _remoteEndPoint;
		private NetworkStream _stream;
		private TcpClient _client;

    /// <summary>
    ///   Release unamanaged resources.
    /// </summary>
    public override void Dispose()
    {
      string temp = _client.Client.RemoteEndPoint.ToString();

			string fileName = $"{temp.Substring(0,temp.IndexOf(':'))}_{DateTime.Now.ToString("yyyyMMddHHmmssff")}";
      File.WriteAllLines($"{SettingsView.BaseDir}{fileName}.txt", _allMessages);
      _allMessages = null;
      _stream.Close();
      _stream.Dispose();
      _client.Close();
      _client.Dispose();
      GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Checks whether the connection has been ternimated.
    /// </summary>
    /// <returns> True if the connection has been termiated and false if it has not been</returns>
    private bool CheckForConnection()
    {
      return (_client.Client.Poll(1000, SelectMode.SelectRead) && _client.Client.Available == 0);
    }
    /// <summary>
    ///   Gets the description of the specific listener type.
    /// </summary>
    /// <returns> a very short description of the type of listener.</returns>
    override public string GetDescription()
    {
      return s_description;
    }
    /// <summary>
    ///   Checks the client for new inbound messages.
    /// </summary>
    override public void CheckForMessages()
    {

      string s ="";
      if (_stream.DataAvailable)
      {
        byte[]? data = new byte[400];
        _stream.Read(data, 0, data.Length);
        s = Encoding.UTF8.GetString(data);
      }
      else
      {
        bool v = CheckForConnection();
        if (v)
        {
          _rm(this);
          return;
        }
      }
      if (s != "")
      {
        _allMessages.Add(s);
        _callback(new SyslogIpModel(_remoteEndPoint,s,true));
      }
      else
      {

      }
    }

    /// <summary>
    ///   Creates an instance of TcpSessionListener that listens based on the passed TcpClient.
    /// </summary>
    /// <param name="client">The underlying connection for this listener. Since Tcp requires potentially multiple connections, </param>
    /// <param name="callback">Method to be invoked and have the message be passed to whenever the listener recieves messages.</param>
    /// <param name="rm">invokes this callback whenever the session is terminated</param>
    public TcpSessionListener(TcpClient client, Action<SyslogIpModel> callback, Action<TcpSessionListener> rm)
    {
      this._client = client;
      this._callback = callback;
      _stream = client.GetStream();
      this._rm = rm;
      _remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
      if (_remoteEndPoint == null) 
      {
        rm(this);
      }
      _allMessages = new List<string>();

		}
  }
}

