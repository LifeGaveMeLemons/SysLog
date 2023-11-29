using SysLog.UI.UiElements.SetFilters;

namespace SysLog.UI.UiElements.SettingsView
{
	internal class SettingsView : NavClass
	{
		public static string BaseDir = "C:/";
		private static SettingsView s_instance;

		/// <summary>
		/// Creates an instance of the SettingsView class using the Singleton pattern.
		/// </summary>
		/// <returns>The instance of SettingsView.</returns>
		public static SettingsView Create()
		{
			if (s_instance == null)
			{
				s_instance = new SettingsView();
			}
			return s_instance;
		}
		private void GetDirectory()
		{
			while (true)
			{
				Console.WriteLine("please enter the directory you would like to save all captures to");
				string input = Console.ReadLine();
				if (Directory.Exists(input))
				{
					BaseDir = input;
					return;
				}
			}
		}
		/// <summary>
		/// Constructor for the SettingsView class. Initializes sub-elements.
		/// </summary>
		private SettingsView()
		{
			_subElements = new List<StringFunctionModel>()
			{
				new StringFunctionModel("Colours",ColourDefinitionView.Create().Load),
				new StringFunctionModel("Filtering",FilterDefinition.Create().Load),
				new StringFunctionModel("Set Listening Address", ListeningIpAddressView.Create().Load),
				new StringFunctionModel("Set saving directory",)
				new StringFunctionModel("exit",Exit)
			};
		}
	}
}
