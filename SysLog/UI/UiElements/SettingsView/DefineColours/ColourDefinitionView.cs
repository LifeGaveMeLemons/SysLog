namespace SysLog.UI.UiElements
{
	internal class ColourDefinitionView : NavClass
	{
		private int _maxValue;
		private int _currentValue = 0;
		private static ColourDefinitionView s_instance;
		private static ConsoleColor[] _colors;
		/// <summary>
		/// Creates an instance of ColourDefinitionView class using the Singleton pattern.
		/// </summary>
		/// <returns>The instance of ColourDefinitionView.</returns>
		public static ColourDefinitionView Create()
		{
			if (s_instance == null)
			{
				s_instance = new ColourDefinitionView();
			}
			return s_instance;
		}

		/// <summary>
		/// Loads the ColourDefinitionView.
		/// </summary>
		public override void Load()
		{
			base.Load();
		}

		/// <summary>
		/// Starts the navigation within the ColourDefinitionView.
		/// </summary>
		internal override void StartNavigation()
		{
			// Create sub-elements based on colors and set up navigation
			_subElements = Handlers.Handler.colors.Select((color, index) => new StringFunctionModel($"severity:{index} - {color.ToString()}", SetColor)).ToList();
			_subElements.Add(new StringFunctionModel("Exit", Exit));
			_maxValue = _subElements.Count - 1;

			Console.ForegroundColor = ConsoleColor.White;
			foreach (StringFunctionModel val in _subElements)
			{
				Console.WriteLine(val);
			}
			Console.CursorVisible = false;
			while (_isRunning)
			{
				switch (Console.ReadKey().Key)
				{
					// Handle navigation key presses
					case ConsoleKey.DownArrow:
						RemoveColor(_currentValue);
						if (_currentValue == _maxValue)
						{
							_currentValue = 0;
						}
						else
						{
							_currentValue++;
						}
						SetColor(_currentValue);
						break;
					case ConsoleKey.UpArrow:
						RemoveColor(_currentValue);
						if (_currentValue == 0)
						{
							_currentValue = _maxValue;
						}
						else
						{
							_currentValue--;
						}
						SetColor(_currentValue);
						break;
					case ConsoleKey.Enter:
						_subElements[_currentValue].Method.Invoke();
						return;
					default:
						continue;
				}
			}
		}

		/// <summary>
		/// Sets the color for a severity level.
		/// </summary>
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
						Handlers.Handler.colors[_currentValue] = _colors[number - 1];
					}
					else
					{
						Console.WriteLine($"please enter a number between 0 and {numberOfElements}");
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
					Console.WriteLine($"please enter a number between 0 and {numberOfElements}");
				}
				catch (Exception e)
				{
					Console.WriteLine("unknown error");
					Console.WriteLine(e.ToString());
				}
			}
		}

		// Private constructor to enforce Singleton pattern
		private ColourDefinitionView()
		{
			_colors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>().ToArray();
		}
	}
}
