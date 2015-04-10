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
	class HiddenElement : BasicPageElement
	{
		/// <summary>
		/// The By Locator
		/// </summary>
		public By By { get; set; }


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
			By = byLocator;

		}
		
		/// <summary>
		/// Get a css value from the element
		/// </summary>
		/// <param name="propertyName">the css property</param>
		/// <returns>the value for the css property</returns>
		public string GetCssValue(string propertyName)
		{
			return WebDriver.WaitForElement(By, Visibility.Hidden).GetCssValue(propertyName);
		}

		/// <summary>
		/// Get a attribute from the element
		/// </summary>
		/// <param name="attributeName">the attribute name</param>
		/// <returns>the value for the attribute</returns>
		public string GetAttribute(string attributeName)
		{
			return WebDriver.WaitForElement(By, Visibility.Hidden).GetAttribute(attributeName);
		}

		/// <summary>
		/// The size of the element
		/// </summary>
		public Size Size
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
		public bool IsElementDisplayed()
		{
			return WebDriver.IsElementDisplayed(By);
		}

		/// <summary>
		/// Get the inner text of an element
		/// </summary>
		/// <returns>the inner text of the element</returns>
		public string GetInnerText()
		{
			TestLog.Add("ReadInnerText: " + By);
			return WebDriver.WaitForElement(By, Visibility.Hidden).Text;
		}
	}
}
