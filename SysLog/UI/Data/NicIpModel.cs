using System.Net;

namespace SysLog.UI.Data
{
	/// <summary>
	/// Represents a model for storing NIC (Network Interface Card) and IP Address information.
	/// </summary>
	internal class NicIpModel
	{
		public string NicName { get; set; }

		/// <summary>
		/// Gets or sets the IP Address associated with the NIC.
		/// </summary>
		public IPAddress IPAddress { get; set; }

		/// <summary>
		/// Initializes a new instance of the NicIpModel class.
		/// </summary>
		/// <param name="nicName">The name of the NIC.</param>
		/// <param name="iPAddress">The IP Address associated with the NIC.</param>
		public NicIpModel(string nicName, IPAddress iPAddress)
		{
			NicName = nicName;
			IPAddress = iPAddress;
		}

		/// <summary>
		/// Implicitly converts a NicIpModel object to a string.
		/// </summary>
		/// <param name="m">The NicIpModel object to convert.</param>
		/// <returns>A formatted string representation of the NicIpModel.</returns>
		public static implicit operator string(NicIpModel m)
		{
			return $"{m.NicName} -- {m.IPAddress}";
		}
	}
}
