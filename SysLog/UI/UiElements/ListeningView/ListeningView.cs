using System.Collections.Concurrent;
using Microsoft.CodeAnalysis.Scripting;
using SysLog.UI.Data.DelimitedMessageModel;
using SysLog.UI.UiElements.SettingsView;

namespace SysLog.UI.UiElements
{
  internal class ListeningView : NavClass
  {
    private static ListeningView s_instance;
    private List<DelimitedMessageModel> _values;

    private ConcurrentQueue<DelimitedMessageModel> _queue;
		private bool _isUpdated = false;
		private int _maxValue;
    private object _maxValuekey = new object();

    public Script<bool>? Filter;

    /// <summary>
    ///   Singleton creation method ensures onlt one active instance at a time.
    /// </summary>
    /// <returns>An active instance of ListeningView.</returns>
		public static ListeningView Create()
    {
      if (s_instance == null)
      {
        s_instance = new ListeningView();
      }
      return s_instance;
    }

    /// <summary>
    /// Exits to previous UI element.
    /// </summary>
    internal override void Exit()
    {
      _isRunning = false;
      _values = null;
      _queue = null;
    }

    /// <summary>
    /// Loads this UI component.
    /// </summary>
    public override void Load()
    {
      _maxValue = -1;
      _values = new List<DelimitedMessageModel>();
      _queue = new ConcurrentQueue<DelimitedMessageModel>();
      Handlers.Handler.Create().SetCallback(RecieveValue);
      base.Load();
    }
    /// <summary>
    /// Adds a new message to the queue, ready to be filtered.
    /// </summary>
    /// <param name="v">The message.</param>
    private void RecieveValue(DelimitedMessageModel v)
    {
      if (_isRunning)
      {
        _isUpdated = true;
        _queue.Enqueue(v);
      }
    }
    /// <summary>
    /// Gets values ready to be displayed.
    /// </summary>
    /// <returns>One value that is a message.</returns>
    private IEnumerable<DelimitedMessageModel> CheckForNewValues()
    {
      DelimitedMessageModel v;
      if (_queue.TryDequeue(out v))
      {

        yield return v;
      }
      else yield break;

    }
    /// <summary>
    ///   begins the stage at which the user can interact witht he UI
    /// </summary>
    internal override void StartNavigation()
    {
      int currentValue = 0;
      while (_isRunning)
      {
        if (Filter == null)
        {
          foreach (DelimitedMessageModel item in CheckForNewValues())
          {
						_values.Add(item);
						Console.ForegroundColor = Handlers.Handler.colors[item.SeverityCalc];

            lock (_maxValuekey)
            {
              _maxValue++;
            }
            Console.SetCursorPosition(0, _maxValue);
            Console.WriteLine((string)item);
            Console.SetCursorPosition(0, currentValue);
          }
        }
        else
        {
					foreach (DelimitedMessageModel item in CheckForNewValues())
					{
            bool v = Filter.RunAsync(item).Result.ReturnValue;
						if (v)
            {
							_values.Add(item);
							Console.ForegroundColor = Handlers.Handler.colors[item.SeverityCalc];

              lock (_maxValuekey)
              {
                _maxValue++;
              }
              Console.SetCursorPosition(0, _maxValue);
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
              if (_maxValue < 0)
              {
                break;
              }
              RemoveColor(currentValue, _values[currentValue].SeverityCalc);
              if (currentValue == _maxValue)
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
              if (_maxValue < 0)
              {
                break;
              }
              RemoveColor(currentValue, _values[currentValue].SeverityCalc);
              if (currentValue == 0)
              {
                currentValue = _maxValue;
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
    /// <summary>
    /// Resets the colour at the specified index.
    /// </summary>
    /// <param name="pos">Index at which the elemtn is located.</param>
    /// <param name="sev">Severity whose color will take palce at the specified loaction.</param>
    internal void RemoveColor(int pos, byte sev)
    {
      Console.SetCursorPosition(0, pos);
      Console.ForegroundColor = Handlers.Handler.colors[sev];
      Console.WriteLine((string)_values[pos]);
    }

    /// <summary>
    /// Sets colour of specified position to dark green.
    /// </summary>
    /// <param name="pos">Position.</param>
    internal override void SetColor(int pos)
    {
      Console.SetCursorPosition(0, pos);
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine((string)_values[pos]);
    }
    /// <summary>
    /// Saves all values to a file.
    /// </summary>
    private void SaveValues()
    {
      _isRunning = false;
			string fileName = $"Listening_Session_{DateTime.Now.ToString("yyyyMMddHHmmssff")}";
			File.WriteAllLines($"{SettingsView.SettingsView.BaseDir}{fileName}.txt", _values.Select((v) => (string) v));
		}
    /// <summary>
    /// Private constructor to enforce singleton reliability.
    /// </summary>
    private ListeningView()
    {    }
  }
}