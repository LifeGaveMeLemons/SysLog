using SysLog.Listeners;
using SysLog.ListenerCol;
using SysLog.Exceptions;
using SysLog.Handlers;
using System.Net;

namespace SysLog
{
  delegate void OnDataRecieved(string data);
  internal class Program
  {
    static void Main(string[] args)
    {
      Handler h = new Handler();
      ListenerCollection collection = new ListenerCollection();
      collection.Add(new UdpListener(h.Enqueue));
      Console.ReadLine();
      
    }
  }
}