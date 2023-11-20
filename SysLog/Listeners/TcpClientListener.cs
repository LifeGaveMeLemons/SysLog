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
    private async Task<bool> CheckForConnection()
    {
      return (client.Client.Poll(1000, SelectMode.SelectRead) && client.Client.Available == 0);
    }

    override public string GetDescription()
    {
      return description;
    }
    override async public void CheckForMessages()
    {
      bool v = await CheckForConnection();
      string s ="";
      if (stream.DataAvailable)
      {
        byte[]? data = new byte[400];
        stream.Read(data, 0, data.Length);
        s = Encoding.UTF8.GetString(data);
      }

     Console.WriteLine(v);
      if (v)
      {
        rm(this);
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

