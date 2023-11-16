using SysLog.Listeners;
using SysLog.ListenerCol;
using SysLog.Exceptions;
using SysLog.Handlers;
using System.Net;
using System.Net.Sockets;

namespace SysLog
{
  delegate void OnDataRecieved(string data);
  delegate void RemoveConnection(TcpClientListener l);
  internal class Program
  {
    static void Main(string[] args)
    {
      Handler h = new Handler();
      ListenerCollection collection = new ListenerCollection(h.Enqueue);
      collection.Add(new UdpListener(h.Enqueue,514));
      collection.AddListener(514);
      Console.ReadLine();
      
    }
  }
}