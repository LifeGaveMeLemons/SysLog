using SysLog.UI.Data;
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
    private static string description = "Tcp Handler";

    private Action<SyslogIpModel> callback;
    private Action<TcpSessionListener> rm;
    IPEndPoint remoteEndPoint;
    NetworkStream stream;
    TcpClient client;

    /// <summary>
    ///   Release unamanaged resources.
    /// </summary>
    public override void Dispose()
    {
      stream.Close();
      stream.Dispose();
      client.Close();
      client.Dispose();
      GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Checks whether the connection has been ternimated.
    /// </summary>
    /// <returns> True if the connection has been termiated and false if it has not been</returns>
    private bool CheckForConnection()
    {
      return (client.Client.Poll(1000, SelectMode.SelectRead) && client.Client.Available == 0);
    }
    /// <summary>
    ///   Gets the description of the specific listener type.
    /// </summary>
    /// <returns> a very short description of the type of listener.</returns>
    override public string GetDescription()
    {
      return description;
    }
    /// <summary>
    ///   Checks the client for new inbound messages.
    /// </summary>
    override public void CheckForMessages()
    {

      string s ="";
      if (stream.DataAvailable)
      {
        byte[]? data = new byte[400];
        stream.Read(data, 0, data.Length);
        s = Encoding.UTF8.GetString(data);
      }
      else
      {
        bool v = CheckForConnection();
        if (v)
        {
          rm(this);
          return;
        }
      }
      if (s != "")
      {
        callback(new SyslogIpModel(remoteEndPoint,s,0));
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
      this.client = client;
      this.callback = callback;
      stream = client.GetStream();
      this.rm = rm;
      remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
      if (remoteEndPoint == null) 
      {
        rm(this);
      }
    }
  }
}

