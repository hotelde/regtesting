using System.Security.Principal;

namespace RegTesting.Mvc.Extensions
{
	/// <summary>
	/// Extensions for IIdentity
	/// </summary>
	public static class IdentityExtensions
	{

		/// <summary>
		/// Get the Domain of a IIdentity
		/// </summary>
		/// <param name="identity">the IIdentity to use</param>
		/// <returns>the Domainpart of identity</returns>
		public static string GetDomain(this IIdentity identity)
		{
			string s = identity.Name;
			int stop = s.IndexOf("\\");
			return (stop > -1) ? s.Substring(0, stop) : string.Empty;
		}

		/// <summary>
		/// Get the Loginname of a IIdentity
		/// </summary>
		/// <param name="identity">the IIdentity to use</param>
		/// <returns>the Loginpart of identity</returns>
		public static string GetLogin(this IIdentity identity)
		{
			string s = identity.Name;
			int stop = s.IndexOf("\\");
			return (stop > -1) ? s.Substring(stop + 1, s.Length - stop - 1) : string.Empty;
		}
	}
}