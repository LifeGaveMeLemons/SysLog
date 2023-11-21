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
    ConsoleColor[] colors;
    public Handler()
    {
      colors = new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Black, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Black, ConsoleColor.Yellow, ConsoleColor.Red }; 
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
        try
        {
          if (strings.TryDequeue(out string v))
          {
            int number = Convert.ToInt32(v.Substring(0, v.IndexOf('>') - 1));
            Console.ForegroundColor = colors[number];
            Console.WriteLine(v);
            
            
          }
          else
          {
            Thread.Sleep(500);
          }
        }
        catch (Exception e ){ Console.WriteLine(e); }
      }
    }
  }
}
