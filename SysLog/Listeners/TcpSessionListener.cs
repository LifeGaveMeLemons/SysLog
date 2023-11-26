using SysLog.UI.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SysLog.Listeners
{
  internal class TcpSessionListener : Listener
  {
    private static string description = "Tcp Handler";

    private Action<SyslogIpModel> callback;
    private Action<TcpSessionListener> rm;
    IPEndPoint remoteEndPoint;
    NetworkStream stream;
    TcpClient client;

    public override void Dispose()
    {
      stream.Close();
      stream.Dispose();
      client.Close();
      client.Dispose();
      GC.SuppressFinalize(this);
    }
    private bool CheckForConnection()
    {
      return (client.Client.Poll(1000, SelectMode.SelectRead) && client.Client.Available == 0);
    }

    override public string GetDescription()
    {
      return description;
    }
    override  public void CheckForMessages()
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

