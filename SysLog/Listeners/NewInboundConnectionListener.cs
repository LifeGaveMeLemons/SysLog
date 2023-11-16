using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SysLog.Listeners
{
  internal class NewInboundConnectionListener
  {
    TcpListener listener;


    public NewInboundConnectionListener(int port)
    {
      listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
      listener.Start();
    }
     public TcpClient? CheckForConnections()
    {
      if (listener.Pending())
      {
                Console.WriteLine( "pend");
                return listener.AcceptTcpClient();
      }
      else return null;
    }


  }
}
