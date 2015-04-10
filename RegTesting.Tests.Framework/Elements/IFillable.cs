using System;

namespace RegTesting.Tests.Framework.Elements
{
	public interface IFillable
	{
		void Type(string text);
		void Type(DateTime dateTime);
	}
}
