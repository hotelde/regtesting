using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
    /// <summary>
    /// Provides instances of the <see cref="By"/> object to the attributes.
    /// </summary>
    internal static class ByFactory
    {
        /// <summary>
        /// Gets an instance of the <see cref="By"/> class based on the specified attribute.
        /// </summary>
        /// <param name="attribute">The <see cref="OpenQA.Selenium.Support.PageObjects.FindsByAttribute"/> describing how to find the element.</param>
        /// <returns>An instance of the <see cref="By"/> class.</returns>
		public static By From(LocateAttribute attribute)
        {
            How how = attribute.How;
            string strUsingValue = attribute.Using;
            switch (how)
            {
                case How.Id:
                    return By.Id(strUsingValue);
                case How.Name:
                    return By.Name(strUsingValue);
                case How.TagName:
                    return By.TagName(strUsingValue);
                case How.ClassName:
                    return By.ClassName(strUsingValue);
                case How.CssSelector:
                    return By.CssSelector(strUsingValue);
                case How.LinkText:
                    return By.LinkText(strUsingValue);
                case How.PartialLinkText:
                    return By.PartialLinkText(strUsingValue);
                case How.XPath:
                    return By.XPath(strUsingValue);
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Did not know how to construct How from how {0}, using {1}", how, strUsingValue));
        }
    }
}