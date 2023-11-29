using SysLog.ListenerCol;
using SysLog.Handlers;
using SysLog.Listeners;

namespace SysLog.UI.UiElements
{
	internal class MainMenu : NavClass
	{
		private static MainMenu _instance;

		public static List<Listener> ListenerList;
		private Handler Handler;
		private ListenerCollection _col;

		/// <summary>
		/// Creates an instance of the MainMenu class using the Singleton pattern.
		/// </summary>
		/// <returns>The instance of MainMenu.</returns>
		public static MainMenu Create()
		{
			if (_instance == null)
			{
				_instance = new MainMenu();
			}
			return _instance;
		}

		/// <summary>
		/// Constructor for the MainMenu class. Initializes handlers, collections, and sub-elements.
		/// </summary>
		private MainMenu()
		{
			Handler = Handler.Create();
			_col = ListenerCollection.Create(Handler.Enqueue);
			_subElements = new List<StringFunctionModel> {
				new StringFunctionModel("Listeners",SysLog.UI.ListenerManagement.Create().Load),
				new StringFunctionModel("View Messages", ListeningView.Create().Load),
				new StringFunctionModel("Settings",SettingsView.SettingsView.Create().Load),
				new StringFunctionModel("Exit",Exit)
			};
		}
	}
}
