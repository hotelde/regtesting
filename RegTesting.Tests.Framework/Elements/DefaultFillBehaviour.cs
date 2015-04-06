using System;
using System.Globalization;
using System.Threading;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	public class DefaultFillBehaviour : IFillable
	{
		private readonly BasicPageElement _objPageElement;

		public DefaultFillBehaviour(BasicPageElement objPageElement)
		{
			_objPageElement = objPageElement;
		}

		public void Type(string strText)
		{
			TestLog.Add("Fill: " + _objPageElement.By + " -> " + strText);
			_objPageElement.WebDriver.WaitForElement(_objPageElement.By).Type(strText);
		}

		public void Type(DateTime datValue)
		{
			CultureInfo objCurrent = Thread.CurrentThread.CurrentUICulture;
			string strDateString = datValue.ToString("d", objCurrent);
			TestLog.Add("FillDate (use culture " + objCurrent + "): " + _objPageElement.By + " -> " + strDateString);
			_objPageElement.WebDriver.WaitForElement(_objPageElement.By).Type(strDateString);
		}
	}
}
