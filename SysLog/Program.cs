using SysLog.UI.UiElements;

namespace SysLog
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.BackgroundColor = ConsoleColor.Black;
      MainMenu m = MainMenu.Create();
      m.Load();
    }
  }
}