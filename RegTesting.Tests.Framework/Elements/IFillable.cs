using System;

namespace RegTesting.Tests.Framework.Elements
{
	public interface IFillable
	{
		void Type(string strText);
		void Type(DateTime datDateTime);
	}
}
