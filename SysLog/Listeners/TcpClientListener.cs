using System.Net.Sockets;
using System.Text;

namespace SysLog.Listeners
{
  internal class TcpClientListener : Listener, IDisposable
  {
    private static string description = "Tcp Handler";

    private OnDataRecieved? callback;
    private RemoveConnection rm;

    NetworkStream stream;
    TcpClient client;


    public void Dispose()
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
        Console.WriteLine(v);
        if (v)
        {
          rm(this);
          return;
        }
      }
      if (s != "")
      {
        callback(s);
      }
      else
      {

      }
    }

    public TcpClientListener(TcpClient client, OnDataRecieved? callback, RemoveConnection rm)
    {
      this.client = client;
      this.callback = callback;
      stream = client.GetStream();
      this.rm = rm;
    }
  }
}

