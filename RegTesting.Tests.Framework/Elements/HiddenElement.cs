using System.Drawing;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	/// <summary>
	/// Represents an Hidden Element for example a Hidden Input
	/// </summary>
	public class HiddenElement : BasicPageElement
	{



		/// <summary>
		/// Construct a new HiddenElement
		/// </summary>
		/// <param name="objBy">The By Locator.</param>
		/// <param name="webDriver">The WebDriver.</param>
		/// <param name="waitModel">Wait-Options for this element.</param>
		/// <param name="parentPageObject">The <see cref="BasePageObject"/> this <see cref="BasicPageElement"/> belongs to.</param>
		/// <param name="clickBehaviour">The ClickBehaviour.</param>
		public HiddenElement(By byLocator, IWebDriver webDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviour = ClickBehaviours.Default)
			: base(byLocator, webDriver, waitModel, parentPageObject, clickBehaviour)
		{
		}
		
		/// <summary>
		/// Get a css value from the element
		/// </summary>
		/// <param name="propertyName">the css property</param>
		/// <returns>the value for the css property</returns>
		public new string GetCssValue(string propertyName)
		{
			return WebDriver.WaitForElement(By, Visibility.Hidden).GetCssValue(propertyName);
		}

		/// <summary>
		/// Get a attribute from the element
		/// </summary>
		/// <param name="attributeName">the attribute name</param>
		/// <returns>the value for the attribute</returns>
        public new string GetAttribute(string attributeName)
		{
			return WebDriver.WaitForElement(By, Visibility.Hidden).GetAttribute(attributeName);
		}

		/// <summary>
		/// The size of the element
		/// </summary>
        public new Size Size
		{
			get
			{
				return WebDriver.WaitForElement(By, Visibility.Hidden).Size;
			}
		}

		/// <summary>
		/// Check if an element is displayed
		/// </summary>
		/// <returns>A bool if the element is displayed</returns>
        public new bool IsElementDisplayed(int timeoutInSeconds = 3)
		{
            return WebDriver.IsElementDisplayed(By, timeoutInSeconds);
		}

		/// <summary>
		/// Get the inner text of an element
		/// </summary>
		/// <returns>the inner text of the element</returns>
        public new string GetInnerText()
		{
			TestLog.Add("ReadInnerText: " + By);
			return WebDriver.WaitForElement(By, Visibility.Hidden).Text;
		}
	}
}
