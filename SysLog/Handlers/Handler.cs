using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Net;
using SysLog.UI.Data;

namespace SysLog.Handlers
{
  
  internal class Handler
  {
    object valuesListKey;
    private static Handler instance;

    ConcurrentQueue<SyslogIpModel> valuesToProcess;

    Thread HandlerThread;
    bool IsRunning = true;
    public static ConsoleColor[] colors;

    Action<SyslogIpModel>? listeningView; 
    private Handler()
    {

      colors = new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Red }; 
      valuesToProcess = new ConcurrentQueue<SyslogIpModel>();
      HandlerThread = new Thread(Process);
      HandlerThread.Start();
    }
    public void SetCallback(Action<SyslogIpModel>? c)
    {
      if (c == null) 
      {
        Stop() ;
        return ;
      }
      listeningView = c;
      Start();
    }
    public void Start()
    {
      if (!IsRunning)
      {
        valuesToProcess = new ConcurrentQueue<SyslogIpModel>();
        IsRunning = true;
        HandlerThread = new Thread(Process);
      }

    }
    public void Stop()
    {
      if (IsRunning)
      {
        IsRunning = false;
        HandlerThread.Join();
        HandlerThread = null;
        valuesToProcess = null;
      }
    }
    public void Enqueue(SyslogIpModel s)
    {
      if (!IsRunning)
      {
        return;
      }
        valuesToProcess.Enqueue(s);
    }

    public void Process()
    {

      while (IsRunning) 
      {
        try
        {
          if (valuesToProcess.TryDequeue(out SyslogIpModel v))
          {

            string numString = v.msg.Substring(1, v.msg.IndexOf('>')-1);
            int number = Convert.ToInt32(numString != "" ? numString : 0);
            v.severity = (byte) (number % 8);
            if (listeningView != null)
            {
              listeningView(v);
            }

          }
          else
          {
            Thread.Sleep(100);
          }
        }
        catch (Exception e ){ Console.WriteLine(e); }
      }
      
    }
    public static Handler Create()
    {
      if (instance == null)
      {
        instance = new Handler();
      }
      return instance;
    }


  }
}
