using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SysLog.Exceptions;

namespace SysLog.Listeners
{

  internal class UdpListener : Listener
  {
    private static string descriiption = "A udp listener";
    private UdpClient client;
    private OnDataRecieved dataCallback;


    //assign empty lambd to avoid null checks
    public OnDataRecieved OnRecieve { set{ dataCallback = value == null?(string val)=> { }:value; } }
    override public string GetDescription()
    {
      return  "";
    }


    override public void CheckForMessages()
    {
      IPEndPoint ip = new IPEndPoint(IPAddress.Any, 514);
      byte[]? bytes = client.Receive(ref ip);
      string resultData = Encoding.UTF8.GetString(bytes);
      if (resultData == "")
      {
        return;
      }
      dataCallback(resultData);

    }
    public UdpListener(OnDataRecieved? callback)
    {
      this.OnRecieve = callback;
      client = new UdpClient(514);
    }
  }
}
