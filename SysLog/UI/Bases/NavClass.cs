﻿using SysLog.UI.UiElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI
{
  /// <summary>
  ///   Serves as a base for most of the UI elements in this program, implementing fundamental navigtion methods that the UI utilizes.
  /// </summary>
  abstract class NavClass
  {

    internal List<StringFunctionModel> subElements;
    internal bool isRunning = true;

    /// <summary>
    ///   Serves as the enry opint to any UI element.
    /// </summary>
    public  virtual void Load()
    {
      isRunning = true;
      while (isRunning)
      {
        Console.Clear();
        StartNavigation();
      }
      return;
    }

    /// <summary>
    ///   Removes colour settings at specified index without Re-rendering the entire CLI.
    /// </summary>
    /// <param name="pos"></param>
    internal virtual void RemoveColor(int pos)
    {
      Console.SetCursorPosition(0, pos);
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine(subElements[pos]);
    }

    /// <summary>
    ///   Sets colour of line to dark green.
    /// </summary>
    /// <param name="pos"></param>
    internal virtual void SetColor(int pos)
    {
      Console.SetCursorPosition(0, pos);
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine(subElements[pos]);
    }

    /// <summary>
    ///   begins the navigation around the specific UI element
    /// </summary>
    internal virtual void StartNavigation()
    {

      int MaxValue = subElements.Count - 1;
      int currentValue = 0;
      Console.ForegroundColor = ConsoleColor.White;
      foreach (StringFunctionModel val in subElements)
      {
        Console.WriteLine(val);
      }
      Console.CursorVisible = false;
      Console.SetCursorPosition(0,0);
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

    /// <summary>
    ///   Serves as a method that terminates subsequent loops and returns to the invoking UI element.
    /// </summary>
    internal virtual void Exit()
    {
      isRunning = false;
    }
  }
}
