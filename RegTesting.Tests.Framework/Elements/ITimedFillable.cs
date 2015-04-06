using System;

namespace RegTesting.Tests.Framework.Elements
{
	public interface ITimedFillable
	{
		void Type(string text, TimeSpan timeToWaitAfterTyping);

		void Type(DateTime dateTime, TimeSpan timeToWaitAfterTyping);
	}
}
