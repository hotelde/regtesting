using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace RegTesting.Tests.Framework.Logic
{
	static class XPathGenerator
	{
		public static String Generate(IWebElement childElement, String current = "")
		{
			String childTag = childElement.TagName;
			if (childTag.Equals("html"))
			{
				return "/html[1]" + current;
			}
			IWebElement parentElement = childElement.FindElement(By.XPath(".."));
			ReadOnlyCollection<IWebElement> childrenElements = parentElement.FindElements(By.XPath("*"));
			int count = 0;
			for (int i = 0; i < childrenElements.Count(); i++)
			{
				IWebElement childrenElement = childrenElements[i];
				String childrenElementTag = childrenElement.TagName;
				if (childTag.Equals(childrenElementTag))
				{
					count++;
				}
				if (childElement.Equals(childrenElement))
				{
					return Generate(parentElement, "/" + childTag + "[" + count + "]" + current);
				}
			}
			return string.Empty;
		}
	}
}
