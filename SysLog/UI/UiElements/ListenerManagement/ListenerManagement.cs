using System;
using System.Collections.Generic;
using SysLog.UI.UiElements;

namespace SysLog.UI
{
	/// <summary>
	/// Represents a management class for handling listeners in the UI.
	/// </summary>
	internal class ListenerManagement : NavClass
	{
		private static ListenerManagement s_instance;

		/// <summary>
		/// Creates a new instance of the ListenerManagement class.
		/// </summary>
		private ListenerManagement()
		{
			_subElements = new List<StringFunctionModel>()
						{
								new StringFunctionModel("Remove Listeners", ViewListeners.Create().Load),
								new StringFunctionModel("Create a listener", CreateListenerView.Create().Load),
								new StringFunctionModel("Exit", Exit)
						};
		}

		/// <summary>
		/// Creates a singleton instance of the ListenerManagement class.
		/// </summary>
		/// <returns>The singleton instance of ListenerManagement.</returns>
		public static ListenerManagement Create()
		{
			if (s_instance == null)
			{
				s_instance = new ListenerManagement();
			}
			return s_instance;
		}
	}
}
