using SysLog.Listeners;
using SysLog.ListenerCol;
using SysLog.Exceptions;
using SysLog.Handler;
using System.Net;

namespace SysLog
{
  delegate void OnDataRecieved(string data);
  internal class Program
  {
    static void Main(string[] args)
    {
      Handler.Handler h = new Handler.Handler();
      OnDataRecieved handle = h.Handle;
      ListenerCollection collection = new ListenerCollection();
      collection.Add(new UdpListener(handle));
      
    }
  }
}