using System;
using RegTesting.Tests.Framework.Properties;

namespace RegTesting.Tests.Framework.Logic
{
	public class WaitAttribute : Attribute
	{
		private int _beforePerformAction = Settings.Default.WaitBeforePerformAction;
		private int _afterPerformAction = Settings.Default.WaitAfterPerformAction;

		/// <summary>
		/// Wait after performing a action
		/// </summary>
		public int AfterPerformAction
		{
			get { return _afterPerformAction; }
			set { _afterPerformAction = value; }
		}

		/// <summary>
		/// Wait after performing a action
		/// </summary>
		public int BeforePerformAction
		{
			get { return _beforePerformAction; }
			set { _beforePerformAction = value; }
		}
	}
}
