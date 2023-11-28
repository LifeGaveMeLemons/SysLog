using System.Collections.Concurrent;
using Microsoft.CodeAnalysis.Scripting;
using SysLog.UI.Data.DelimitedMessageModel;

namespace SysLog.UI.UiElements
{
  internal class ListeningView : NavClass
  {
    private static ListeningView instance;
    List<DelimitedMessageModel> values;

    ConcurrentQueue<DelimitedMessageModel> queue;
    bool isUpdated = false;
    int MaxValue;
    object maxValuekey = new object();

    public Script<bool>? filter;


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
      IsRunning = false;
      values = null;
      queue = null;
    }
    public override void Load()
    {
      MaxValue = -1;
      values = new List<DelimitedMessageModel>();
      queue = new ConcurrentQueue<DelimitedMessageModel>();
      Handlers.Handler.Create().SetCallback(RecieveValue);
      base.Load();
    }
    private void RecieveValue(DelimitedMessageModel v)
    {
      if (IsRunning)
      {
        isUpdated = true;
        queue.Enqueue(v);
      }
    }
    private IEnumerable<DelimitedMessageModel> CheckForNewValues()
    {
      DelimitedMessageModel v;
      if (queue.TryDequeue(out v))
      {

        yield return v;
      }
      else yield break;

    }
    internal override void StartNavigation()
    {
      int currentValue = 0;
      while (IsRunning)
      {
        if (filter == null)
        {
          foreach (DelimitedMessageModel item in CheckForNewValues())
          {
						values.Add(item);
						Console.ForegroundColor = Handlers.Handler.colors[item.SeverityCalc];

            lock (maxValuekey)
            {
              MaxValue++;
            }
            Console.SetCursorPosition(0, MaxValue);
            Console.WriteLine((string)item);
            Console.SetCursorPosition(0, currentValue);
          }
        }
        else
        {
					foreach (DelimitedMessageModel item in CheckForNewValues())
					{
            bool v = filter.RunAsync(item).Result.ReturnValue;
						if (v)
            {
							values.Add(item);
							Console.ForegroundColor = Handlers.Handler.colors[item.SeverityCalc];

              lock (maxValuekey)
              {
                MaxValue++;
              }
              Console.SetCursorPosition(0, MaxValue);
              Console.WriteLine((string)item);
              Console.SetCursorPosition(0, currentValue);
            }
					}
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
              RemoveColor(currentValue, values[currentValue].SeverityCalc);
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
              RemoveColor(currentValue, values[currentValue].SeverityCalc);
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
      IsRunning = false;
      File.WriteAllLines("D:/new.txt",values.Select((v) =>(string) v ).ToArray());
    }
    private ListeningView()
    {    }
  }
}