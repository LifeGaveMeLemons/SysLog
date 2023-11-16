using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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


    override public string GetDescription()
    {
      return description;
    }
    override public void CheckForMessages()
    {
      if (client.Connected)
      {

        if (!stream.DataAvailable)
        {
          return;
        }
        byte[]? data = new byte[400];
        stream.Read(data, 0, data.Length);
        string s = Encoding.UTF8.GetString(data);
        if (s != "")
        {
          callback(s);
        }
        else
        {
          rm(this);
        }
      }
      else
      {
        rm(this);
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

