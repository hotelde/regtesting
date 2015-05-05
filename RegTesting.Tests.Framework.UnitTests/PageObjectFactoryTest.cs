using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Logic;
using RegTesting.Tests.Framework.UnitTests.PageElements;
using RegTesting.Tests.Framework.UnitTests.PageObjects;

namespace RegTesting.Tests.Framework.UnitTests
{
	[TestClass]
	public class PageObjectFactoryTest
	{

		/// <summary>
		/// The PageObjectFactory should be able to create an empty PageObject.
		/// </summary>
		[TestMethod]
		public void ShouldBeAbleToCreateEmptyPageObject()
		{
			EmptyPageObject emptyPageObject = PageObjectFactory.GetPageObjectByType<EmptyPageObject>(null);

			Assert.AreEqual(emptyPageObject.GetType(), typeof(EmptyPageObject));
		}

		/// <summary>
		/// The PageObjectFactory should be able to create a PageObject with just a BasicPageElement.
		/// </summary>
		[TestMethod]
		public void ShouldBeAbleToCreatePageObjectWithBasicPageElement()
		{
			BasicPageObject basicPageObject = PageObjectFactory.GetPageObjectByType<BasicPageObject>(null);

			Assert.AreEqual(basicPageObject.GetType(), typeof(BasicPageObject));
			Assert.AreEqual(basicPageObject.Element.GetType(), typeof(BasicPageElement));

		}

		/// <summary>
		/// The PageObjectFactory should be able to create a PageObject with default page elements.
		/// </summary>
		[TestMethod]
		public void ShouldBeAbleToCreatePageObjectWithDefaultPageElements()
		{
			DefaultElementsPageObject defaultElementsPageObject = PageObjectFactory.GetPageObjectByType<DefaultElementsPageObject>(null);

			Assert.AreEqual(defaultElementsPageObject.GetType(), typeof(DefaultElementsPageObject));
			Assert.AreEqual(defaultElementsPageObject.ButtonElement.GetType(), typeof(Button));
			Assert.AreEqual(defaultElementsPageObject.CheckBoxElement.GetType(), typeof(CheckBox));
			Assert.AreEqual(defaultElementsPageObject.HiddenElement.GetType(), typeof(HiddenElement));
			Assert.AreEqual(defaultElementsPageObject.ImageElement.GetType(), typeof(Image));
			Assert.AreEqual(defaultElementsPageObject.InputElement.GetType(), typeof(Input));
			Assert.AreEqual(defaultElementsPageObject.LinkElement.GetType(), typeof(Link));
			Assert.AreEqual(defaultElementsPageObject.SelectBoxElement.GetType(), typeof(SelectBox));
		}


		/// <summary>
		/// The PageObjectFactory should be able to create a PageObject with derived page elements.
		/// </summary>
		[TestMethod]
		public void ShouldBeAbleToCreatePageObjectWithDerivedPageElements()
		{
			DerivedElementsPageObject derivedElementsPageObject = PageObjectFactory.GetPageObjectByType<DerivedElementsPageObject>(null);

			Assert.AreEqual(derivedElementsPageObject.GetType(), typeof(DerivedElementsPageObject));
			Assert.AreEqual(derivedElementsPageObject.BasicPageElement.GetType(), typeof(DerivedBasicPageElement));
			Assert.AreEqual(derivedElementsPageObject.ButtonElement.GetType(), typeof(DerivedButton));
			Assert.AreEqual(derivedElementsPageObject.CheckBoxElement.GetType(), typeof(DerivedCheckBox));
			Assert.AreEqual(derivedElementsPageObject.HiddenElement.GetType(), typeof(DerivedHiddenElement));
			Assert.AreEqual(derivedElementsPageObject.ImageElement.GetType(), typeof(DerivedImage));
			Assert.AreEqual(derivedElementsPageObject.InputElement.GetType(), typeof(DerivedInput));
			Assert.AreEqual(derivedElementsPageObject.LinkElement.GetType(), typeof(DerivedLink));
			Assert.AreEqual(derivedElementsPageObject.SelectBoxElement.GetType(), typeof(DerivedSelectBox));
		}

		/// <summary>
		/// The PageObjectFactory should be able to create a PageObject with an included PageObject that contains another page element.
		/// </summary>
		[TestMethod]
		public void ShouldBeAbleToCreatePageObjectWithIncludedPartialPageObject()
		{
			ParentPageObject parentPageObject = PageObjectFactory.GetPageObjectByType<ParentPageObject>(null);

			Assert.AreEqual(parentPageObject.GetType(), typeof(ParentPageObject));
			Assert.AreEqual(parentPageObject.Element.GetType(), typeof(BasicPageElement));
			Assert.AreEqual(parentPageObject.ChildPageObject.Element.GetType(), typeof(BasicPageElement));

		}
	}

}
