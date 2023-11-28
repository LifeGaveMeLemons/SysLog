using SysLog.ListenerCol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SysLog.Handlers;
using SysLog.Listeners;
using SysLog.UI;
using SysLog.UI.UiElements;
namespace SysLog.UI.UiElements
{
  internal class MainMenu : NavClass
  {
    private static MainMenu instance;

    public static List<Listener> listenerList;
    private Handler handler;
    private ListenerCollection col;

    public static MainMenu Create()
    {
      if (instance == null)
      {
        instance = new MainMenu();
      }
      return instance;
    }

    private MainMenu()
    {
      handler = Handler.Create();
      col = ListenerCollection.Create(handler.Enqueue);
      subElements = new List<StringFunctionModel> {
        new StringFunctionModel("Listeners",SysLog.UI.ListenerManagement.Create().Load),
        new StringFunctionModel("View Messages", ListeningView.Create().Load),
        new StringFunctionModel("Settings",SettingsView.SettingsView.Create().Load),
        new StringFunctionModel("Exit",Exit)
      };



    }
    }
}
