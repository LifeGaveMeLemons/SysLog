using SysLog.UI.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.UiElements
{
  internal class ListeningView : NavClass
  {
    private static ListeningView instance;
    List<SyslogIpModel> values;

    ConcurrentQueue<SyslogIpModel> queue;
    bool isUpdated = false;
    int MaxValue;
    object maxValuekey = new object();
    public static ListeningView Create()
    {
      if (instance == null)
      {
        instance = new ListeningView();
      }
      return instance;
    }
    internal override void Exit()
    {
      isRunning = false;
      values = null;
      queue = null;
    }
    public override void Load()
    {
      MaxValue = -1;
      values = new List<SyslogIpModel>();
      queue = new ConcurrentQueue<SyslogIpModel>();
      Handlers.Handler.Create().SetCallback(RecieveValue);
      base.Load();
    }
    private void RecieveValue(SyslogIpModel v)
    {
      isUpdated = true;
      queue.Enqueue(v);
    }
    private IEnumerable<SyslogIpModel> CheckForNewValues()
    {
      SyslogIpModel v;
      if (queue.TryDequeue(out v))
      {
        values.Add(v);
        yield return v;
      }
      else yield break;

    }
    internal override void StartNavigation()
    {
      int currentValue = 0;
      while (isRunning)
      {

        foreach (SyslogIpModel item in CheckForNewValues())
        {
          Console.ForegroundColor = Handlers.Handler.colors[item.severity];
          
          lock(maxValuekey)
          {
            MaxValue++;
          }
          Console.SetCursorPosition(0, MaxValue);
          Console.WriteLine((string)item);
          Console.SetCursorPosition(0, currentValue);
        }
        if (Console.KeyAvailable)
        {
          switch (Console.ReadKey().Key)
          {
            case ConsoleKey.DownArrow:
              if (MaxValue < 0)
              {
                break;
              }
              RemoveColor(currentValue, values[currentValue].severity);
              if (currentValue == MaxValue)
              {
                currentValue = 0;
              }
              else
              {
                currentValue++;
              }
              SetColor(currentValue);
              break;
            case ConsoleKey.UpArrow:
              if (MaxValue < 0)
              {
                break;
              }
              RemoveColor(currentValue, values[currentValue].severity);
              if (currentValue == 0)
              {
                currentValue = MaxValue;
              }
              else
              {
                currentValue--;
              }
              SetColor(currentValue);
              break;
            case ConsoleKey.Escape:
              Exit();
              return;
            case ConsoleKey.Tab:
              SaveValues();
              Exit();
              return;
            default:
              break;
          }
        }
      }
    }
    internal void RemoveColor(int pos, byte sev)
    {
      Console.SetCursorPosition(0, pos);
      Console.ForegroundColor = Handlers.Handler.colors[sev];
      Console.WriteLine((string)values[pos]);
    }
    internal override void SetColor(int pos)
    {
      Console.SetCursorPosition(0, pos);
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine((string)values[pos]);
    }
    private void SaveValues()
    {
      File.WriteAllLines("",values.Select((v) =>(string) v ).ToArray());
    }
    private ListeningView()
    {    }
  }
}