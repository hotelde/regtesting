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
		/// <param name="objWebDriver">The WebDriver.</param>
		/// <param name="waitModel">Wait-Options for this element.</param>
		/// <param name="parentPageObject">The <see cref="BasePageObject"/> this <see cref="BasicPageElement"/> belongs to.</param>
		/// <param name="objClickBehaviour">The ClickBehaviour.</param>
		public HiddenElement(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours objClickBehaviour = ClickBehaviours.Default)
			: base(objBy, objWebDriver, waitModel, parentPageObject, objClickBehaviour)
		{
			By = objBy;

		}
		
		/// <summary>
		/// Get a css value from the element
		/// </summary>
		/// <param name="strPropertyName">the css property</param>
		/// <returns>the value for the css property</returns>
		public string GetCssValue(string strPropertyName)
		{
			return WebDriver.WaitForElement(By, Visibility.Hidden).GetCssValue(strPropertyName);
		}

		/// <summary>
		/// Get a attribute from the element
		/// </summary>
		/// <param name="strAttributeName">the attribute name</param>
		/// <returns>the value for the attribute</returns>
		public string GetAttribute(string strAttributeName)
		{
			return WebDriver.WaitForElement(By, Visibility.Hidden).GetAttribute(strAttributeName);
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
