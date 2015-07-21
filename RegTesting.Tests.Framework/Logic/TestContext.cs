using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegTesting.Tests.Framework.Elements;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// The TestContext
	/// </summary>
	public static class TestContext
	{
		/// <summary>
		/// The click behaviour factory
		/// </summary>
		public static IClickBehaviourFactory ClickBehaviourFactory = new ClickBehaviourFactory();

		/// <summary>
		/// The fill behaviour factory
		/// </summary>
		public static IFillBehaviourFactory FillBehaviourFactory = new FillBehaviourFactory();

	}
}
