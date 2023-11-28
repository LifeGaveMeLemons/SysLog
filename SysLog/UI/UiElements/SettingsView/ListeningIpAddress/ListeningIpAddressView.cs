using SysLog.UI.Data;
using System;
using System.Net;
using System.Net.NetworkInformation;
namespace SysLog.UI.UiElements.SettingsView.ListeningIpAddress
{
	internal class ListeningIpAddressView : NavClass
	{
		NetworkInterface[] networkInterfaces;

		List<NicIpModel> networkInterfaceIpAddresses;

		public override void Load()
		{
			
			networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < networkInterfaces.Length; i++)
			{
				IPAddressInformationCollection data = networkInterfaces[i].GetIPProperties().AnycastAddresses;
				foreach (IPAddressInformation ip in data)
				{
					networkInterfaceIpAddresses.Add(new NicIpModel(networkInterfaces[i].Name, ip.Address));
				}
			}
			base.Load();
		}
	}
}
