using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegTesting.Contracts
{

	/// <summary>
	/// The test is not available exception
	/// </summary>
	public class TestcaseNotAvailableException : Exception
	{
		public TestcaseNotAvailableException(string strMessage) : base(strMessage)
		{
		}
	}
}
