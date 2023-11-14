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

  internal class UdpListener
  {
    private UdpClient client;
    private IPEndPoint ip;
    private OnDataRecieved dataCallback;

    private bool IsListening;
    private bool IsSetLoListen;

    //copy IP address to avoid mutability outside class
    public IPEndPoint Ip
    {
      get
      {
        return new IPEndPoint(ip.Address, ip.Port);
      }
      set
      {
        ip = new IPEndPoint(value.Address, value.Port);
      }
    }
    //assign empty lambd to avoid null checks
    public OnDataRecieved OnRecieve { set{ dataCallback = value == null?(string val)=> { }:value; } }

    public void StartListeing()
    {
      if (IsListening)
      {
        throw new UDPAlreadyListeningException();
      }
      IsSetLoListen = true;
    }
    public void Listen()
    {
      IsListening = true;
      while(IsSetLoListen)
      {
        client
      }
    }
    public UdpListener(IPEndPoint ip,OnDataRecieved? callback)
    {
      this.ip = ip;
      this.OnRecieve = callback;
    }
  }
}
