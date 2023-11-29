using SysLog.UI.UiElements.SetFilters;

namespace SysLog.UI.UiElements.SettingsView
{
	internal class SettingsView : NavClass
	{
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
				new StringFunctionModel("exit",Exit)
			};
		}
	}
}
