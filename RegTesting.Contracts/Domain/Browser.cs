using System;

namespace RegTesting.Contracts.Domain
{

	/// <summary>
	/// A Browser
	/// </summary>
	public class Browser : MarshalByRefObject
	{

		/// <summary>
		/// The ID
		/// </summary>
		public virtual int ID { get; set; }

		/// <summary>
		/// The Name
		/// </summary>
		public virtual string Name { get; set; }

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
