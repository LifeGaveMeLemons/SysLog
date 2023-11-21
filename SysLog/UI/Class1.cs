using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI
{
  internal class UserInterface
  {
    public UserInterface()
    {
      Console.WriteLine(SelectionMenu(new string[] { "hhh", "aaa", "bbb" }));
      Console.ReadLine();
    }

    async private int SelectionMenu(IEnumerable<IConvertible> c)
    {
      if (c == null)
      {
        return 0;
      }
      int maxOption = c.Count();
      int CurrentChoice = 0;
      while (true)
      {
        Console.Clear();

        int CurrentIteration = 0;

        foreach (IConvertible v in c)
        { 
          if (CurrentIteration == CurrentChoice)
          {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(v);
            Console.ResetColor();
          }
          else
          {
            Console.WriteLine(v);
          }
          CurrentIteration++;
        }


        switch (Console.ReadKey().Key)
        {
          case ConsoleKey.DownArrow:
            if (CurrentChoice == maxOption)
            {
              CurrentChoice = 0;
            }
            else
            {
              CurrentChoice++;
            }
            break;
          case ConsoleKey.UpArrow:
            if (CurrentChoice == 0)
            {
              CurrentChoice = maxOption;
            }
            else
            {
              CurrentChoice--;
            }

            break;
          case ConsoleKey.Enter:
            return CurrentChoice;
          default:
            continue;

        }
      }
    }
  }
}
