using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace SysLog.Handlers
{
  internal class Handler
  {
    ConcurrentQueue<string> strings;

    Thread HandlerThread;
    bool IsRunning = true;

    public Handler()
    {
      strings = new ConcurrentQueue<string>();
      HandlerThread = new Thread(Process);
      HandlerThread.Start();
    }
    
    public void Stop()
    {

      IsRunning = false;
    }
    public void Enqueue(string s)
    {
        strings.Enqueue(s);
    }

    public void Process()
    {
      while (IsRunning) 
      {
        if (strings.TryDequeue(out string v))
        {
          Console.WriteLine(v);
        }
        else
        {
          Thread.Sleep(500);
        }
      }
    }
  }
}
