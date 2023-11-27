using SysLog.ListenerCol;
using SysLog.Listeners;
using SysLog.UI.UiElements.ListenerManagement.ViewListeners.ViewSpecificProtocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI
{
  internal class Tcp : NavClass
  {
    private static Tcp instance;
    public static Tcp Create()
    {
      if (instance == null)
      {
        instance = new Tcp();
      }
      return instance;


    }
    ListenerCollection col;

    int currentValue = 0;
    List<InboundConnectionListener> listeners;
    public override void Load()
    {
      isRunning = true;
      while (isRunning)
      {
        col = ListenerCollection.Create(null);
        listeners = col.GetTcpListeners();
        subElements = new List<StringFunctionModel>()
          {
            new StringFunctionModel("Exit",Exit)
          };
        foreach (InboundConnectionListener listener in listeners)
        {
          subElements.Add(new StringFunctionModel(listener.GetDesription(), UdpClient));
        }
        Console.Clear();
        StartNavigation();
      }
      return;
    }
    internal override void StartNavigation()
    {

      int MaxValue = subElements.Count - 1;
      Console.ForegroundColor = ConsoleColor.White;
      foreach (StringFunctionModel val in subElements)
      {
        Console.WriteLine(val);
      }
      Console.CursorVisible = false;
      Console.SetCursorPosition(0, 0);
      Console.ForegroundColor = ConsoleColor.DarkGreen; Console.WriteLine(subElements[0]);
      while (isRunning)
      {

        switch (Console.ReadKey().Key)
        {
          case ConsoleKey.DownArrow:
            RemoveColor(currentValue);
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
            RemoveColor(currentValue);
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
          case ConsoleKey.Enter:
            subElements[currentValue].Method.Invoke();
            return;
          default:
            continue;
        }
      }
    }
    private void UdpClient()
    {
      Console.Clear();
      InboundConnectionListener subject = listeners[currentValue - 1];
      Console.WriteLine($"Port:{subject.Port}");
      Console.WriteLine("would you like to delete this listener?\n Press enter for yes \n press esc for no");
      while (true)
      {
        switch (Console.ReadKey().Key)
        {
          case ConsoleKey.Enter:
            col.RemoveTcpListener(subject);
            currentValue = 0;
            return;
          case ConsoleKey.Escape:
            return;
          default:
            break;
        }
      }
    }
  }
}
