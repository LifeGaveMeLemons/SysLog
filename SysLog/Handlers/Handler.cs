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
    private object _valuesListKey;
    private static Handler s_instance;

    private ConcurrentQueue<SyslogIpModel> _valuesToProcess;

    private Thread _handlerThread;
    private bool _isRunning = true;
    public static ConsoleColor[] colors;

    private Action<DelimitedMessageModel>? _listeningView; 
    /// <summary>
    ///   Creates new instance of Handler.
    /// </summary>
    private Handler()
    {

      colors = new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Red }; 
      _valuesToProcess = new ConcurrentQueue<SyslogIpModel>();
      _handlerThread = new Thread(Process);
      _handlerThread.Start();
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
      _listeningView = c;
      Start();
    }

    /// <summary>
    ///   Starts processing Data
    /// </summary>
    public void Start()
    {
      if (!_isRunning)
      {
        _valuesToProcess = new ConcurrentQueue<SyslogIpModel>();
        _isRunning = true;
        _handlerThread = new Thread(Process);
      }

    }

    /// <summary>
    /// Stops processing data
    /// </summary>
    public void Stop()
    {
      if (_isRunning)
      {
        _isRunning = false;
        _handlerThread.Join();
        _handlerThread = null;
        _valuesToProcess = null;
      }
    }

    /// <summary>
    ///   Adds new inbound messages to queue, if the Handler is running.
    /// </summary>
    /// <param name="s">raw string and src IP data from the message</param>
    public void Enqueue(SyslogIpModel s)
    {
      if (!_isRunning)
      {
        return;
      }
        _valuesToProcess.Enqueue(s);
    }

    /// <summary>
    ///   Processes data that has eben added to the queue. Puts it into a separated format, and passes it further for further processing.
    /// </summary>
    public void Process()
    {

      while (_isRunning) 
      {
        try
        {
          if (_valuesToProcess.TryDequeue(out SyslogIpModel v))
          {
            DelimitedMessageModel m = new DelimitedMessageModel(v.Msg, v.Ip,v.IsTcp);
            if (_listeningView != null)
            {
              _listeningView(m);
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
      if (s_instance == null)
      {
        s_instance = new Handler();
      }
      return s_instance;
    }


  }
}
