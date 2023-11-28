using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.Data
{
	internal class NicIpModel
	{
		string NicName;
		IPAddress IPAddress;

		public NicIpModel(string nicName, IPAddress iPAddress)
		{
			NicName = nicName;
			IPAddress = iPAddress;
		}
		public static implicit operator string (NicIpModel m)
		{
			return $"{m.NicName} -- {m.IPAddress}";
		}
	}
}
