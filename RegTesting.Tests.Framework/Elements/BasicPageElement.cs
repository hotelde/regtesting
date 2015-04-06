using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.Logic.Extensions;

namespace RegTesting.Tests.Framework.Elements
{
	/// <summary>
	/// A BasicPageElement
	/// </summary>
	public class BasicPageElement
	{
		private readonly TimeSpan _waitAfterClick;
		protected IClickable ClickBehaviour;
		private readonly TimeSpan _waitBeforeClick;
		private readonly IEnumerable<LocateOptions> _elementsBeforeClick;
		private readonly IEnumerable<LocateOptions> _elementsAfterClick;

		/// <summary>
		/// The By Locator
		/// </summary>
		public By By { get; set; }

		/// <summary>
		/// The WebDriver
		/// </summary>
		public IWebDriver WebDriver { get; set; }

		public BasePageObject ParentPageObject { get; set; }

		/// <summary>
		/// Construct a new BasicPageElement
		/// </summary>
		/// <param name="objBy">The By Locator.</param>
		/// <param name="objWebDriver">The WebDriver.</param>
		/// <param name="waitModel">Wait-Options for this element.</param>
		/// <param name="parentPageObject">The <see cref="BasePageObject"/> this <see cref="BasicPageElement"/> belongs to.</param>
		/// <param name="objClickBehaviour">The ClickBehaviour.</param>
		public BasicPageElement(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours objClickBehaviour = ClickBehaviours.Default)
		{
			_waitAfterClick = waitModel.WaitAfterAction == 0 ? TimeSpan.Zero : new TimeSpan(0, 0, 0, 0, waitModel.WaitAfterAction);
			_waitBeforeClick = waitModel.WaitBeforeAction == 0 ? TimeSpan.Zero : new TimeSpan(0, 0, 0, 0, waitModel.WaitBeforeAction);
			_elementsBeforeClick = waitModel.WaitForElementsBeforeAction;
			_elementsAfterClick = waitModel.WaitForElementsAfterAction;
			By = objBy;
			WebDriver = objWebDriver;
			ParentPageObject = parentPageObject;
			ClickBehaviour = ClickBehaviourFactory.Create(objClickBehaviour, this);
		}

		/// <summary>
		/// Get a css value from the element
		/// </summary>
		/// <param name="strPropertyName">the css property</param>
		/// <returns>the value for the css property</returns>
		public string GetCssValue(string strPropertyName)
		{
			return WebDriver.WaitForElement(By, Visibility.Any).GetCssValue(strPropertyName);
		}

		/// <summary>
		/// Get a attribute from the element
		/// </summary>
		/// <param name="strAttributeName">the attribute name</param>
		/// <returns>the value for the attribute</returns>
		public string GetAttribute(string strAttributeName)
		{
			return WebDriver.WaitForElement(By, Visibility.Any).GetAttribute(strAttributeName);
		}

		/// <summary>
		/// The size of the element
		/// </summary>
		public Size Size
		{
			get
			{
				return WebDriver.WaitForElement(By).Size;
			}
		}


		/// <summary>
		/// Check if an element is displayed
		/// </summary>
		/// <returns>A bool if the element is displayed</returns>
		public bool IsElementDisplayed(int timeoutInSeconds = 3)
		{
			return WebDriver.IsElementDisplayed(By, timeoutInSeconds);
		}


		/// <summary>
		/// Get the inner text of an element
		/// </summary>
		/// <returns>the inner text of the element</returns>
		public string GetInnerText()
		{
			TestLog.Add("ReadInnerText: " + By);
			return WebDriver.WaitForElement(By).Text;
		}

		public void Click()
		{
			WaitFor(_elementsBeforeClick, When.Before);
			ClickBehaviour.Click(_waitBeforeClick, _waitAfterClick);
			WaitFor(_elementsAfterClick, When.After);
		}

		private void WaitFor(IEnumerable<LocateOptions> waitForElements, When when)
		{
			if (waitForElements != null)
			{
				foreach (LocateOptions locateOptions in waitForElements)
				{
					try
					{
						WebDriver.WaitForElement(locateOptions.By, locateOptions.Visibility);
					}
					catch (TimeoutException e)
					{
						throw new TimeoutException("A error occured after clicking " + By + ". While waiting for a defined related element " + locateOptions.By + ". See inner exception for more details", e);
					}
				}
			}
		}

		public T ClickToPageObject<T>() where T : BasePageObject
		{
			return ClickBehaviour.ClickToPageObject<T>(_waitBeforeClick);
		}


		/// <summary>
		/// Checks if the element has a specific class
		/// </summary>
		/// <param name="classname">class</param>
		/// <returns>Boolean indicating if the element has the specific class</returns>
		public bool HasClass(string classname)
		{
			if (String.IsNullOrEmpty(classname))
				throw new ArgumentException("classname");

			string classnames = GetAttribute("class");
			if (String.IsNullOrEmpty(classnames))
				return false;

			string[] classes = classnames.Split(' ');

			return classes.Contains(classname);

		}


		/// <summary>
		/// Checks if the element has specific classes
		/// </summary>
		/// <param name="classnames">classes</param>
		/// <returns>Boolean indicating if the element has all of the specific classes</returns>
		public bool HasClasses(params string[] classnames)
		{
			return classnames.All(HasClass);
		}
	}
}
