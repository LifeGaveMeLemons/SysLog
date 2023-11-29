using SysLog.UI.Data;
using System.Net;
using System.Net.NetworkInformation;
namespace SysLog.UI.UiElements
{
	internal class ListeningIpAddressView : NavClass
	{
		private static ListeningIpAddressView s_instance;

		/// <summary>
		/// The currently selected listening IP address.
		/// </summary>
		public static IPAddress CurrentListeningAddress = IPAddress.Any;
		private NetworkInterface[] _networkInterfaces;
		private List<NicIpModel> _networkInterfaceIpAddresses;

		private int _maxValue;
		private int _currentValue = 0;

		/// <summary>
		/// Creates an instance of the ListeningIpAddressView class using the Singleton pattern.
		/// </summary>
		/// <returns>The instance of ListeningIpAddressView.</returns>
		public static ListeningIpAddressView Create()
		{
			if (s_instance == null)
			{
				s_instance = new ListeningIpAddressView();
			}
			return s_instance;
		}

		/// <summary>
		/// Loads the ListeningIpAddressView and initializes available IP addresses.
		/// </summary>
		public override void Load()
		{
			_subElements = new List<StringFunctionModel>();
			_networkInterfaceIpAddresses = new List<NicIpModel>();
			_networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < _networkInterfaces.Length; i++)
			{
				if (_networkInterfaces[i].OperationalStatus != OperationalStatus.Up)
				{
					continue;
				}
				IPInterfaceProperties data = _networkInterfaces[i].GetIPProperties();
				foreach (IPAddressInformation ip in data.UnicastAddresses)
				{
					if (ip.IsDnsEligible)
					{
						_networkInterfaceIpAddresses.Add(new NicIpModel(_networkInterfaces[i].Name, ip.Address));
					}

				}
			}
			_networkInterfaceIpAddresses.Add(new NicIpModel("Any Ip address", IPAddress.Any));
			_networkInterfaceIpAddresses.Add(new NicIpModel("Loopback Ip address", IPAddress.Loopback));
			foreach (NicIpModel data in _networkInterfaceIpAddresses)
			{
				_subElements.Add(new StringFunctionModel(data, SetIpAddress));
			}
			_subElements.Add(new StringFunctionModel("Exit", Exit));
			_maxValue = _subElements.Count - 1;
			base.Load();
		}

		/// <summary>
		/// Sets the selected IP address for listening and updates the CurrentListeningAddress field.
		/// </summary>
		private void SetIpAddress()
		{
			ListenerCol.ListenerCollection.Create(null).ChangeIp(_networkInterfaceIpAddresses[_currentValue].IPAddress);
			CurrentListeningAddress = _networkInterfaceIpAddresses[_currentValue].IPAddress;
		}

		/// <summary>
		/// Starts the navigation within the ListeningIpAddressView.
		/// </summary>
		internal override void StartNavigation()
		{
			Console.ForegroundColor = ConsoleColor.White;
			foreach (StringFunctionModel val in _subElements)
			{
				Console.WriteLine(val);
			}
			Console.WriteLine($"Current:{CurrentListeningAddress}");
			Console.CursorVisible = false;
			Console.SetCursorPosition(0, 0);
			Console.ForegroundColor = ConsoleColor.DarkGreen; Console.WriteLine(_subElements[0]);
			while (_isRunning)
			{

				switch (Console.ReadKey().Key)
				{
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

		// Private constructor to enforce Singleton pattern
		private ListeningIpAddressView()
		{

		}
	}
}
