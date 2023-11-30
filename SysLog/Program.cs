using SysLog.UI.UiElements;
using SysLog.UI.UiElements.SettingsView;

namespace SysLog
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.BackgroundColor = ConsoleColor.Black;
      SettingsView.Create().GetDirectory();
      MainMenu m = MainMenu.Create();
      m.Load();
    }
  }
}