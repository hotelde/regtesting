﻿using OpenQA.Selenium;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic;

namespace RegTesting.Tests.Framework.Elements
{
	public class Button : BasicPageElement
	{
		
		public Button(By objBy, IWebDriver objWebDriver, WaitModel waitModel, BasePageObject parentBasePageObject, ClickBehaviours objClickBehaviour = ClickBehaviours.Default)
			: base(objBy, objWebDriver, waitModel, parentBasePageObject, objClickBehaviour)
		{
		}
	}
}