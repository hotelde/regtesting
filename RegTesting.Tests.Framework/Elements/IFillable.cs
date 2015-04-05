using System;

namespace RegTesting.Tests.Framework.Elements
{
	interface IFillable
	{
		void Type(string strText);
		void Type(DateTime datDateTime);
	}
}
