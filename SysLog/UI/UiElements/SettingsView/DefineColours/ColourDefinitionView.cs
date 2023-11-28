using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.UiElements
{
	internal class ColourDefinitionView : NavClass
	{
		int MaxValue;
		int CurrentValue = 0;
		private static ColourDefinitionView insatnce ;
		public static ColourDefinitionView Create()
		{
			if (insatnce == null)
			{
				insatnce = new ColourDefinitionView();
			}
			return insatnce ;
		}
		public override void Load()
		{

			base.Load();
		}
		internal override void StartNavigation()
		{
			subElements = Handlers.Handler.colors.Select((color, index) => new StringFunctionModel($"severity:{index} - {color.ToString()}", SetColor)).ToList();
			subElements.Add(new StringFunctionModel("Exit", Exit));
			MaxValue = subElements.Count - 1;

			Console.ForegroundColor = ConsoleColor.White;
			foreach (StringFunctionModel val in subElements)
			{
				Console.WriteLine(val);
			}
			Console.CursorVisible = false;
			while (IsRunning)
			{

				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.DownArrow:
						RemoveColor(CurrentValue);
						if (CurrentValue == MaxValue)
						{
							CurrentValue = 0;
						}
						else
						{
							CurrentValue++;
						}
						SetColor(CurrentValue);
						break;
					case ConsoleKey.UpArrow:
						RemoveColor(CurrentValue);
						if (CurrentValue == 0)
						{
							CurrentValue = MaxValue;
						}
						else
						{
							CurrentValue--;
						}
						SetColor(CurrentValue);
						break;
					case ConsoleKey.Enter:
						subElements[CurrentValue].Method.Invoke();
						return;
					default:
						continue;
				}
			}
		}
		private void SetColor()
		{
			int numberOfElements = 1;
			Console.Clear();
			foreach (string color in Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>().Select((en) => en.ToString()))
			{
				Console.WriteLine($"{numberOfElements}){color}");
				numberOfElements++;
			}
			Console.WriteLine("please select the colour you would like to assign to this severity level");
			while (true)
			{
				try
				{
					string input = Console.ReadLine();
					int number = Convert.ToInt32(input);
					if (number > 0 && number < numberOfElements)
					{
						Handlers.Handler.colors[CurrentValue] = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>().ToArray()[number-1] ;
					}
					else
					{
						Console.WriteLine($"please enter a  number between 0 and {numberOfElements}");
						continue;
					}
					break;
				}
				catch (FormatException)
				{
					Console.WriteLine("please enter a number");
				}
				catch (OverflowException)
				{
					Console.WriteLine($"please enter a  number between 0 and {numberOfElements}");
				}
				catch(Exception e) 
				{
					Console.WriteLine("unknown error");
					Console.WriteLine(e.ToString()); 
				}
			}

		}
		public ColourDefinitionView()
		{
		}
		
	}
}
