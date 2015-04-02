using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegTesting.Node
{
	/// <summary>
	/// Configuration of node
	/// </summary>
	public class NodeConfiguration
	{
		/// <summary>
		/// the pollingIntervall of the node
		/// </summary>
		public static int PollingIntervall
		{
			get
			{
				string pollingIntervall = ConfigurationManager.AppSettings["PollingIntervall"];
				return String.IsNullOrWhiteSpace(pollingIntervall) ? 5000 : int.Parse(pollingIntervall);
			}
		}

		/// <summary>
		/// the address to the hosted node service
		/// </summary>
		public static string ServerAddress
		{
			get
			{
				return ConfigurationManager.AppSettings["ServerAddress"];
			}
		}
	}
}
