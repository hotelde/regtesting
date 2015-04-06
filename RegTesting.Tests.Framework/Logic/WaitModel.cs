using System.Collections.Generic;
using RegTesting.Tests.Framework.Elements;

namespace RegTesting.Tests.Framework.Logic
{
	public class WaitModel
	{
		public WaitModel()
		{
			WaitForElementsAfterAction = new LocateOptions[0];
			WaitForElementsBeforeAction = new LocateOptions[0];
		}
		/// <summary>
		/// The time in milliseconds waited before an action on an element is triggered.
		/// </summary>
		public int WaitBeforeAction { get; set; }

		/// <summary>
		/// The time in milliseconds waited after an action on an element is triggered.
		/// </summary>
		public int WaitAfterAction { get; set; }
		
		/// <summary>
		/// Wait for this elements with the specific <see cref="LocateOptions"/> BEFORE an action on an element is triggered.
		/// </summary>
		public IEnumerable<LocateOptions> WaitForElementsBeforeAction { get; set; }

		/// <summary>
		/// Wait for this elements with the specific <see cref="LocateOptions"/> AFTER an action on an element is triggered.
		/// </summary>
		public IEnumerable<LocateOptions> WaitForElementsAfterAction { get; set; } 

	}
}
