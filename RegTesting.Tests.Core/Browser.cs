using System;

namespace RegTesting.Tests.Core
{

	/// <summary>
	/// A Browser
	/// </summary>
	public class Browser : MarshalByRefObject
	{

		/// <summary>
		/// The Browserstring
		/// </summary>
		public virtual string Browserstring { get; set; }

		/// <summary>
		/// The Versionsstring
		/// </summary>
		public virtual string Versionsstring { get; set; }

	}
}
