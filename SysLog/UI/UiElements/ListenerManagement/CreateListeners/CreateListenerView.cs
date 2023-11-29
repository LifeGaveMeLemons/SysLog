using SysLog.ListenerCol;
using SysLog.Listeners;
using System.Net.Sockets;

namespace SysLog.UI.UiElements
{
	/// <summary>
	/// Represents a view for creating listeners (UDP and TCP) in the UI.
	/// </summary>
	internal class CreateListenerView : NavClass
	{
		private static CreateListenerView s_instance;

		/// <summary>
		/// Creates a new instance of the CreateListenerView class.
		/// </summary>
		private CreateListenerView()
		{
			_subElements = new List<StringFunctionModel>()
						{
								new StringFunctionModel("Create a UDP Listener", this.CreateUDPListener),
								new StringFunctionModel("Create a TCP listener", this.CreateTCPListener),
								new StringFunctionModel("Exit", this.Exit)
						};
		}

		/// <summary>
		/// Creates a singleton instance of the CreateListenerView class.
		/// </summary>
		/// <returns>The singleton instance of CreateListenerView.</returns>
		public static CreateListenerView Create()
		{
			if (s_instance == null)
			{
				s_instance = new CreateListenerView();
			}
			return s_instance;
		}

		/// <summary>
		/// Creates a UDP listener.
		/// </summary>
		private void CreateUDPListener()
		{
			ushort portToBeCreatedOn = 514;
			Console.Clear();
			Console.WriteLine("What port do you want to create the UDP listener on?");
			bool isGettingInput = true;

			while (isGettingInput)
			{
				try
				{
					portToBeCreatedOn = Convert.ToUInt16(Console.ReadLine());
					isGettingInput = false;
					Handlers.Handler h = Handlers.Handler.Create();
					UdpListener listener = new UdpListener(h.Enqueue, portToBeCreatedOn);
					ListenerCollection.Create(null).Add(listener);
					return;
				}
				catch (FormatException)
				{
					Console.WriteLine("Please enter a valid number.");
				}
				catch (OverflowException)
				{
					Console.WriteLine($"Please enter a number between 0 and {ushort.MaxValue}.");
				}
				catch (SocketException)
				{
					Console.WriteLine("There is already a UDP listener on this socket.");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		/// <summary>
		/// Creates a TCP listener.
		/// </summary>
		private void CreateTCPListener()
		{
			ushort portToBeCreatedOn = 514;
			Console.Clear();
			Console.WriteLine("What port do you want to create the TCP listener on?");
			bool isGettingInput = true;

			while (isGettingInput)
			{
				try
				{
					portToBeCreatedOn = Convert.ToUInt16(Console.ReadLine());
					isGettingInput = false;
					ListenerCollection.Create(null).AddIncomingConnectionListener(portToBeCreatedOn);
				}
				catch (FormatException)
				{
					Console.WriteLine("Please enter a number.");
				}
				catch (OverflowException)
				{
					Console.WriteLine($"Please enter a number between 0 and {ushort.MaxValue}.");
				}
				catch (SocketException)
				{
					Console.WriteLine("There is already a UDP listener on this socket.");
				}
			}
		}
	}
}
