using System.Net;

namespace SysLog.UI.Data
{
	/// <summary>
	/// Represents a model for storing Syslog information including IP endpoint, message, and protocol information.
	/// </summary>
	internal class SyslogIpModel
	{
		/// <summary>
		/// Gets or sets the IP endpoint associated with the Syslog.
		/// </summary>
		public IPEndPoint Ip { get; set; }

		/// <summary>
		/// Gets or sets the message associated with the Syslog.
		/// </summary>
		public string Msg { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Syslog is using TCP protocol (true) or not (false).
		/// </summary>
		public bool IsTcp { get; set; }

		/// <summary>
		/// Initializes a new instance of the SyslogIpModel class.
		/// </summary>
		/// <param name="ip">The IP endpoint associated with the Syslog.</param>
		/// <param name="msg">The message associated with the Syslog.</param>
		/// <param name="isTcp">A value indicating whether the Syslog is using TCP protocol (true) or not (false).</param>
		public SyslogIpModel(IPEndPoint ip, string msg, bool isTcp)
		{
			Ip = ip;
			Msg = msg;
			IsTcp = isTcp;
		}
	}
}
