using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SysLog.Listeners
{
  internal class InboundConnectionListener
  {

    TcpListener listener;

    public InboundConnectionListener(int port)
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
