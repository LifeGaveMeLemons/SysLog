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
    private UdpClient client;
    private OnDataRecieved dataCallback;

    private bool IsListening;
    private bool IsSetToListen;

    //assign empty lambd to avoid null checks
    public OnDataRecieved OnRecieve { set{ dataCallback = value == null?(string val)=> { }:value; } }
    override public string GetDescription()
    {
      return  "";
    }
    override public void StartListeing()
    {
      if (IsListening)
      {
        throw new UDPAlreadyListeningException();
      }
      IsSetToListen = true;
      Listen();
    }
    override public void StopListening()
    {
      IsSetToListen = false;
    }
    override public void Listen()
    {
      IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
      IsListening = true;
      while(IsSetToListen)
      {
        byte[] bytes = client.Receive(ref ip);
        string resultData = Encoding.UTF8.GetString(bytes);
        dataCallback(resultData);
      }
      IsListening = false;
    }
    public UdpListener(OnDataRecieved? callback)
    {
      this.OnRecieve = callback;
    }
  }
}
