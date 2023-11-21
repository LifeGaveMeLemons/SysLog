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
    List<string>
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
      Console.BackgroundColor = ConsoleColor.White;
    }
    public void Start()
    {
      if (!IsRunning)
      {
        IsRunning = true;
        strings = new ConcurrentQueue<string>();
        HandlerThread = new Thread(Process);
      }

    }
    public void Stop()
    {
      if (IsRunning)
      {
        IsRunning = false;
        HandlerThread = null;
        strings = null;
      }
    }
    public void Enqueue(string s)
    {
      if (!IsRunning)
      {
        return;
      }
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

            string numString = v.Substring(1, v.IndexOf('>')-1);
            int number = Convert.ToInt32( numString != "" ? numString:0);
            Console.ForegroundColor = colors[number%8];
            Console.WriteLine(v);
            Console.WriteLine("stgheitgwhw");
            
            
          }
          else
          {
            Thread.Sleep(100);
          }
        }
        catch (Exception e ){ Console.WriteLine(e); }
      }
      Console.WriteLine("exit");
    }
    
  }
}
