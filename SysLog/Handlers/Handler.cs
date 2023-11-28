using System.Collections.Concurrent;
using SysLog.UI.Data;
using SysLog.UI.Data.DelimitedMessageModel;

namespace SysLog.Handlers
{
  /// <summary>
  /// Handled inbound messages.
  /// </summary>
  internal class Handler
  {
    object valuesListKey;
    private static Handler instance;

    ConcurrentQueue<SyslogIpModel> valuesToProcess;

    Thread HandlerThread;
    bool IsRunning = true;
    public static ConsoleColor[] colors;

    Action<DelimitedMessageModel>? listeningView; 
    /// <summary>
    ///   Creates new instance of Handler.
    /// </summary>
    private Handler()
    {

      colors = new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Red }; 
      valuesToProcess = new ConcurrentQueue<SyslogIpModel>();
      HandlerThread = new Thread(Process);
      HandlerThread.Start();
    }

    /// <summary>
    ///   Sets Delegate to pass first processed data to.
    /// </summary>
    /// <param name="c">Delegate that takes DelimitedMessageModel as a param</param>
    public void SetCallback(Action<DelimitedMessageModel>? c)
    {
      if (c == null) 
      {
        Stop() ;
        return ;
      }
      listeningView = c;
      Start();
    }

    /// <summary>
    ///   Starts processing Data
    /// </summary>
    public void Start()
    {
      if (!IsRunning)
      {
        valuesToProcess = new ConcurrentQueue<SyslogIpModel>();
        IsRunning = true;
        HandlerThread = new Thread(Process);
      }

    }

    /// <summary>
    /// Stops processing data
    /// </summary>
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

    /// <summary>
    ///   Adds new inbound messages to queue, if the Handler is running.
    /// </summary>
    /// <param name="s">raw string and src IP data from the message</param>
    public void Enqueue(SyslogIpModel s)
    {
      if (!IsRunning)
      {
        return;
      }
        valuesToProcess.Enqueue(s);
    }

    /// <summary>
    ///   Processes data that has eben added to the queue. Puts it into a separated format, and passes it further for further processing.
    /// </summary>
    public void Process()
    {

      while (IsRunning) 
      {
        try
        {
          if (valuesToProcess.TryDequeue(out SyslogIpModel v))
          {
            DelimitedMessageModel m = new DelimitedMessageModel(v.msg, v.ip);
            if (listeningView != null)
            {
              listeningView(m);
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

    /// <summary>
    ///   Singleton class. Uses static method to create or copy refrence.
    /// </summary>
    /// <returns>New instance of Hanlder in one doesent exists, otherwise returns the instance currently active.</returns>
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
