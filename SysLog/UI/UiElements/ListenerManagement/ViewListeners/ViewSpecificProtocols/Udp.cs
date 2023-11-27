using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysLog.Listeners;
using SysLog.ListenerCol; 

namespace SysLog.UI.UiElements.ListenerManagement.ViewListeners.ViewSpecificProtocols
{
  internal class Udp : NavClass
  {
    private static Udp instance;
    public static Udp Create()
    {

      if (instance == null)
      {
        instance = new Udp();
      }
      return instance;
    }
    ListenerCollection col;
    int currentValue = 0;
    List<UdpListener> listeners;
    public override void Load()
    {
      isRunning = true;

        col = ListenerCollection.Create(null);
      while (isRunning)
      {
        listeners = col.GetUdpList();
        subElements = new List<StringFunctionModel>()
          {
            new StringFunctionModel("Exit",Exit)
          };
        foreach (UdpListener listener in listeners)
        {
          subElements.Add(new StringFunctionModel(listener.GetDescription(), UdpClient));
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
      while (base.isRunning)
      {

        switch (Console.ReadKey().Key)
        {
          case ConsoleKey.DownArrow:
            base.RemoveColor(currentValue);
            if (currentValue == MaxValue)
            {
              currentValue = 0;
            }
            else
            {
              currentValue++;
            }
            base.SetColor(currentValue);
            break;
          case ConsoleKey.UpArrow:
            base.RemoveColor(currentValue);
            if (currentValue == 0)
            {
              currentValue = MaxValue;
            }
            else
            {
              currentValue--;
            }
            base.SetColor(currentValue);
            break;
          case ConsoleKey.Enter:
            subElements[currentValue].Method.Invoke();
            return;
          default:
            continue;
        }
      }
    }

    public Udp()
    {


    }
    private void UdpClient()
    {
      Console.Clear();
      UdpListener subject = listeners[currentValue -1];
      Console.WriteLine($"Port:{subject.Port}");
      Console.WriteLine("would you like to delete this listener?\n Press enter for yes \n press esc for no");
      while (true)
      {
        switch (Console.ReadKey().Key)
        {
          case ConsoleKey.Enter:
            col.RemoveClient(subject);
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
