using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using RegTesting.Tests.Framework.Elements;
using RegTesting.Tests.Framework.Enums;
using RegTesting.Tests.Framework.Logic.PageSettings;
using RegTesting.Tests.Framework.Properties;

namespace RegTesting.Tests.Framework.Logic
{
	internal class PageObjectFactory
	{
		private static readonly IPageSettingsFactory PageSettingsFactory;
		static PageObjectFactory()
		{
			PageSettingsFactory = new PageSettingsFactory();
		}

		/// <summary>
		/// Creates the and navigate to.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objWebDriver">The web driver.</param>
		/// <param name="strBaseUrl">The base URL.</param>
		/// <param name="bolSuppressLanguageParameter">Supresses the language parameter</param>
		/// <param name="arrFurtherUrlParameters">
		/// The further URL param after the lng param. 
		/// The example input value "cpn=2553 would navigate us to: www.hotel.de?lng=de&cpn=2553
		/// </param>
		/// <returns>A PageObject, navigated to the PageUrl.</returns>
		public static T CreateAndNavigateTo<T>(IWebDriver objWebDriver, string strBaseUrl, bool bolSuppressLanguageParameter = false, params string[] arrFurtherUrlParameters) where T : BasePageObject
		{
			T pageObject = CreatePageObject<T>(objWebDriver);
			Type objType = typeof(T);
			PagePropsAttribute objPageAttribute = (PagePropsAttribute)objType.GetCustomAttribute(typeof(PagePropsAttribute), true);
			string pageUrl = strBaseUrl;
			if (!pageUrl.EndsWith("/"))
				pageUrl = pageUrl + "/";
			
			if (pageObject.PageSettings.IsSeoRoute)
			{
				pageUrl = CreateSeoRoute(pageUrl, objPageAttribute, arrFurtherUrlParameters);
			}
			else
			{	
				pageUrl = CreateRoute(pageUrl,objPageAttribute, bolSuppressLanguageParameter, arrFurtherUrlParameters);
			}

			TestLog.AddWithoutTime("<br><b>>>>" + objType.Name + "</b>");
			TestLog.Add("CreateAndNavigate: " + objType.Name + " -> " + pageUrl);
			objWebDriver.Navigate().GoToUrl(pageUrl);
			return pageObject;
		}

		public static T CreateAndNavigateTo<T>(IWebDriver objWebDriver, string strBaseUrl, params string[] arrFurtherUrlParameters) where T : BasePageObject
		{
			return CreateAndNavigateTo<T>(objWebDriver, strBaseUrl, false, arrFurtherUrlParameters);
		}

		private static string CreateSeoRoute(string pageUrl, PagePropsAttribute objPageAttribute, params string[] arrFurtherUrlParameters)
		{
			string seoRoute = objPageAttribute != null && objPageAttribute.PageUrl != null ? objPageAttribute.PageUrl.ToLower() : string.Empty;

			List<string> lstRemainingParams = new List<string>();
			foreach (string param in arrFurtherUrlParameters)
			{
				string key = param.Split('=')[0].ToLower();
				string value = param.Split('=')[1];

				if (seoRoute.Contains("{" + key + "}"))
				{
					seoRoute = seoRoute.Replace("{" + key + "}", value);
				}
				else
				{
					lstRemainingParams.Add(param);
				}
			}
			return string.Concat(pageUrl, Thread.CurrentThread.CurrentUICulture.Name, "/", seoRoute, GetUrlParams(lstRemainingParams.ToArray()));

		}

		private static string CreateRoute(string pageUrl, PagePropsAttribute objPageAttribute, bool bolSuppressLanguageParameter = false, params string[] arrFurtherUrlParameters)
		{
			string returnUrl = string.Concat(pageUrl, objPageAttribute != null && objPageAttribute.PageUrl != null ? objPageAttribute.PageUrl : string.Empty);
			returnUrl = string.Concat(returnUrl, GetUrlParams(arrFurtherUrlParameters));
			if (!bolSuppressLanguageParameter)
				returnUrl = string.Concat(returnUrl, string.Format("&lng={0}", Thread.CurrentThread.CurrentUICulture.Name));
			return returnUrl;
		}

		private static string GetUrlParams(string[] arrUrlParameters)
		{
			string returnVal = "?ddcc=1";
			string parameters = string.Join("&", arrUrlParameters);
			if (!string.IsNullOrEmpty(parameters))
				returnVal = string.Concat(returnVal, "&", parameters);
			return returnVal;
		}

		
		public static T GetPageObjectByType<T>(IWebDriver objWebDriver) where T : BasePageObject
		{
			TestLog.AddWithoutTime("<br><b>>>>" + typeof(T).Name + "</b>");
			TestLog.Add("GetPageObjectByType: " + typeof(T).Name);
			return CreatePageObject<T>(objWebDriver);
		}

		private static T CreatePageObject<T>(IWebDriver objWebDriver) where T : BasePageObject
		{
			try
			{
				T pageObject = (T)Activator.CreateInstance(typeof(T), objWebDriver, PageSettingsFactory);

				InitElements(objWebDriver, pageObject);
				
				TestLog.Add("Applying Page settings for '" + pageObject.GetType().Name + "'");
				pageObject.PageSettings.ApplySettings();
				
				return pageObject;
			}
			catch (TargetInvocationException objException)
			{
				if (objException.InnerException != null) throw objException.InnerException;
				throw;
			}
		}

		private static void InitElements(IWebDriver driver, BasePageObject objPageObject)
		{
			if (objPageObject == null)
			{
				throw new ArgumentNullException("objPageObject");
			}

			Type objType = objPageObject.GetType();
			List<MemberInfo> lstMemberInfos = new List<MemberInfo>();
			const BindingFlags publicBindingOptions = BindingFlags.Instance | BindingFlags.Public;
			lstMemberInfos.AddRange(objType.GetFields(publicBindingOptions));
			lstMemberInfos.AddRange(objType.GetProperties(publicBindingOptions));

			while (objType != null)
			{
				const BindingFlags nonPublicBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic;
				lstMemberInfos.AddRange(objType.GetFields(nonPublicBindingOptions));
				lstMemberInfos.AddRange(objType.GetProperties(nonPublicBindingOptions));
				objType = objType.BaseType;
			}

			Type pageObjectType = objPageObject.GetType();
			foreach (MemberInfo objMember in lstMemberInfos)
			{
				LocateAttribute objLocateAttribute = GetAttribute<LocateAttribute>(objMember);
				ClickBehaviourAttribute objClickBehaviourAttribute = GetAttribute<ClickBehaviourAttribute>(objMember);
				ClickBehaviours enmClickBehaviour = objClickBehaviourAttribute != null
					                                    ? objClickBehaviourAttribute.Using
					                                    : ClickBehaviours.Default;

				FillBehaviourAttribute fillBehaviourAttribute = GetAttribute<FillBehaviourAttribute>(objMember);
				FillBehaviour fillBehaviour = fillBehaviourAttribute != null 
												? fillBehaviourAttribute.Using 
												: FillBehaviour.Default;
				
				WaitAttribute waitAttribute = GetAttribute<WaitAttribute>(objMember);
				WaitForElementsOnActionAttribute waitForElementsOnActionAttribute = GetAttribute<WaitForElementsOnActionAttribute>(objMember);

				WaitModel waitModel = new WaitModel
				{
					WaitBeforeAction = waitAttribute != null
						? waitAttribute.BeforePerformAction
						: Settings.Default.WaitBeforePerformAction,
					WaitAfterAction = waitAttribute != null
						? waitAttribute.AfterPerformAction
						: Settings.Default.WaitAfterPerformAction,
					WaitForElementsBeforeAction = CreateLocateOptionsFromAttribute(waitForElementsOnActionAttribute, pageObjectType, When.Before),
					WaitForElementsAfterAction = CreateLocateOptionsFromAttribute(waitForElementsOnActionAttribute, pageObjectType, When.After),
				};

				if (objLocateAttribute != null)
				{
					By objBy = CreateLocator(objMember);
					object objCreatedElementObject;
					FieldInfo objField = objMember as FieldInfo;
					PropertyInfo objProperty = objMember as PropertyInfo;
					if (objField != null)
					{
						objCreatedElementObject = CreateElementObject(objField.FieldType, driver, objBy, waitModel, objPageObject, enmClickBehaviour, fillBehaviour);
						if (objCreatedElementObject == null)
						{
							throw new ArgumentException("Type of field '" + objField.Name + "' is not IWebElement or IList<IWebElement>");
						}

						objField.SetValue(objPageObject, objCreatedElementObject);
					}
					else if (objProperty != null)
					{
						objCreatedElementObject = CreateElementObject(objProperty.PropertyType, driver, objBy, waitModel, objPageObject, enmClickBehaviour, fillBehaviour);
						if (objCreatedElementObject == null)
						{
							throw new ArgumentException("Type of property '" + objProperty.Name + "' is not IWebElement or IList<IWebElement>");
						}

						objProperty.SetValue(objPageObject, objCreatedElementObject, null);
					}
				}

				PartialPageObjectAttribute objPartialPageObjectAttribute = GetAttribute<PartialPageObjectAttribute>(objMember);
				if (objPartialPageObjectAttribute != null)
				{
					PropertyInfo objProperty = objMember as PropertyInfo;
					if (objProperty != null)
					{

						MethodInfo objCastMethod = typeof(PageObjectFactory).GetMethod("CreatePageObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(objProperty.PropertyType);
						object objCreatedElementObject = objCastMethod.Invoke(null, new object[] { driver });
						if (objCreatedElementObject == null)
						{
							throw new ArgumentException("Type of property '" + objProperty.Name + "' is not IWebElement or IList<IWebElement>");
						}

						objProperty.SetValue(objPageObject, objCreatedElementObject, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,null,null);
					}
				}
			}
		}

		internal static BasicPageElement CreateElementObject(Type objFieldType, IWebDriver objDriver, By objBy, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours enmClickBehaviour = ClickBehaviours.Default, FillBehaviour fillBehaviour = FillBehaviour.Default)
		{
			if (objFieldType == typeof (Button))
			{
				return new Button(objBy, objDriver, waitModel, parentPageObject, enmClickBehaviour);
			}
			if (objFieldType == typeof (Link))
			{
				return new Link(objBy, objDriver, waitModel, parentPageObject, enmClickBehaviour);
			}
			if (objFieldType == typeof(Image))
			{
				return new Image(objBy, objDriver, waitModel, parentPageObject, enmClickBehaviour);
			}
			if (objFieldType == typeof(BasicPageElement))
			{
				return new BasicPageElement(objBy, objDriver, waitModel, parentPageObject);
			}
			if (objFieldType == typeof(SelectBox))
			{
				return new SelectBox(objBy, objDriver, waitModel, parentPageObject);
			}
			if (objFieldType == typeof(CheckBox))
			{
				return new CheckBox(objBy, objDriver, waitModel, parentPageObject, enmClickBehaviour);
			}
			if (objFieldType == typeof(Input))
			{
				return new Input(objBy, objDriver, waitModel, parentPageObject, enmClickBehaviour, fillBehaviour);
			}
			if (objFieldType == typeof(HiddenElement))
			{
				return new HiddenElement(objBy, objDriver, waitModel, parentPageObject, enmClickBehaviour);
			}
			return null;
		}

		public static T GetAttribute<T>(MemberInfo member) where T : Attribute
		{
			Attribute[] arrAttributes = Attribute.GetCustomAttributes(member, typeof(T), true);
			if (arrAttributes.Length > 1)
				throw new ArgumentException("Member " + member.Name + " has " + arrAttributes.Length + " Attributes instead of only one.");
			if (arrAttributes.Length == 0)
				return null;

			Attribute objAttribute = arrAttributes[0];

			return (T) objAttribute;

		}
		
		private static By CreateLocator(MemberInfo member)
		{
			LocateAttribute objCastedAttribute = GetAttribute<LocateAttribute>(member);
			if (objCastedAttribute.Using == null)
			{
				objCastedAttribute.Using = member.Name;

			}

			return objCastedAttribute.Finder;
		}

		private static By CreateLocatorForRelatedElement(string elementName, Type pageObjectType)
		{
			Type type = pageObjectType.GetType();
			bool isChildOfBasePageObject = false;
			while (type != null)
			{
				if (type.BaseType != typeof (BasePageObject))
				{
					isChildOfBasePageObject = true;
				}
				type = type.BaseType;
			}
			if (!isChildOfBasePageObject)
				throw new ArgumentException("Cannot create a related element from a object that does not inherit from " + typeof(BasePageObject));

			MemberInfo[] memberInfos = pageObjectType.GetMembers();

			foreach (MemberInfo memberInfo in memberInfos)
			{
				if (memberInfo.Name.Equals(elementName))
				{
					return CreateLocator(memberInfo);
				}
			}
			throw new Exception("Could not create related element of the name '" + elementName + "' from the pageobject-type '" + pageObjectType + "'.");
		}

		private static IEnumerable<LocateOptions> CreateLocateOptionsFromAttribute(WaitForElementsOnActionAttribute waitForOnActionAttribute, Type pageObjectType, When when)
		{
			if (waitForOnActionAttribute != null && waitForOnActionAttribute.When == when)
			{
				LocateOptions[] locateOptions =
				{
					new LocateOptions
					{
						By = CreateLocatorForRelatedElement(waitForOnActionAttribute.WaitForPageElementWithName, pageObjectType),
						Visibility = waitForOnActionAttribute.Visibility
					}
				};
				return locateOptions;
			}
			return new LocateOptions[0];
		}
	}
}
