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
	public class PageObjectFactory
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
		/// <param name="webDriver">The web driver.</param>
		/// <param name="baseUrl">The base URL.</param>
		/// <param name="suppressLanguageParameter">Supresses the language parameter</param>
		/// <param name="hashTagParams">hash tag parameters</param>
		/// <param name="furtherUrlParameters">
		/// The further URL param after the lng param. 
		/// The example input value "cpn=2553 would navigate us to: www.hotel.de?lng=de&cpn=2553
		/// </param>
		/// <returns>A PageObject, navigated to the PageUrl.</returns>
		public static T CreateAndNavigateTo<T>(IWebDriver webDriver, string baseUrl, bool suppressLanguageParameter = false, string[] hashTagParams = null, params string[] furtherUrlParameters) where T : BasePageObject
		{
			T pageObject = CreatePageObject<T>(webDriver);
			Type type = typeof(T);
			PagePropsAttribute pageAttribute = (PagePropsAttribute)type.GetCustomAttribute(typeof(PagePropsAttribute), true);
			string pageUrl = baseUrl;
			if (!pageUrl.EndsWith("/"))
				pageUrl = pageUrl + "/";

			if (pageObject.PageSettings.IsSeoRoute)
			{
				pageUrl = CreateSeoRoute(pageUrl, pageAttribute, furtherUrlParameters);
			}
			else
			{
				pageUrl = CreateRoute(pageUrl, pageAttribute, suppressLanguageParameter, furtherUrlParameters);
			}

			if (hashTagParams != null)
				pageUrl = string.Concat(pageUrl, "#?", string.Join("&", hashTagParams));

			TestLog.AddWithoutTime("<br><b>>>>" + type.Name + "</b>");
			TestLog.Add("CreateAndNavigate: " + type.Name + " -> " + pageUrl);
			webDriver.Navigate().GoToUrl(pageUrl);
			return pageObject;
		}


		public static T CreateAndNavigateTo<T>(IWebDriver webDriver, string baseUrl, params string[] furtherUrlParameters) where T : BasePageObject
		{
			return CreateAndNavigateTo<T>(webDriver, baseUrl, false, null, furtherUrlParameters);
		}

		private static string CreateSeoRoute(string pageUrl, PagePropsAttribute pageAttribute, params string[] furtherUrlParameters)
		{
			string seoRoute = pageAttribute != null && pageAttribute.PageUrl != null ? pageAttribute.PageUrl.ToLower() : string.Empty;

			List<string> remainingParams = new List<string>();
			foreach (string param in furtherUrlParameters)
			{
				string key = param.Split('=')[0].ToLower();
				string value = param.Split('=')[1];

				if (seoRoute.Contains("{" + key + "}"))
				{
					seoRoute = seoRoute.Replace("{" + key + "}", value);
				}
				else
				{
					remainingParams.Add(param);
				}
			}
			return string.Concat(pageUrl, Thread.CurrentThread.CurrentUICulture.Name, "/", seoRoute, GetUrlParams(remainingParams.ToArray()));

		}

		private static string CreateRoute(string pageUrl, PagePropsAttribute pageAttribute, bool suppressLanguageParameter = false, params string[] furtherUrlParameters)
		{
			string returnUrl = string.Concat(pageUrl, pageAttribute != null && pageAttribute.PageUrl != null ? pageAttribute.PageUrl : string.Empty);
			returnUrl = string.Concat(returnUrl, GetUrlParams(furtherUrlParameters));
			if (!suppressLanguageParameter)
				returnUrl = string.Concat(returnUrl, string.Format("&lng={0}", Thread.CurrentThread.CurrentUICulture.Name));
			return returnUrl;
		}

		private static string GetUrlParams(string[] urlParameters)
		{
			string parameters = string.Join("&", urlParameters);
			if (!string.IsNullOrEmpty(parameters))
				return string.Concat("?", parameters);
			return String.Empty;
		}

		
		public static T GetPageObjectByType<T>(IWebDriver webDriver) where T : BasePageObject
		{
			TestLog.AddWithoutTime("<br><b>>>>" + typeof(T).Name + "</b>");
			TestLog.Add("GetPageObjectByType: " + typeof(T).Name);
			return CreatePageObject<T>(webDriver);
		}

		private static T CreatePageObject<T>(IWebDriver webDriver) where T : BasePageObject
		{
			try
			{
				T pageObject = (T)Activator.CreateInstance(typeof(T), webDriver, PageSettingsFactory);

				InitElements(webDriver, pageObject);
				
				TestLog.Add("Applying Page settings for '" + pageObject.GetType().Name + "'");
				pageObject.PageSettings.ApplySettings();
				
				return pageObject;
			}
			catch (TargetInvocationException exception)
			{
				if (exception.InnerException != null) throw exception.InnerException;
				throw;
			}
		}

		private static void InitElements(IWebDriver driver, BasePageObject pageObject)
		{
			if (pageObject == null)
			{
				throw new ArgumentNullException("pageObject");
			}

			Type type = pageObject.GetType();
			List<MemberInfo> memberInfos = new List<MemberInfo>();
			const BindingFlags publicBindingOptions = BindingFlags.Instance | BindingFlags.Public;
			memberInfos.AddRange(type.GetFields(publicBindingOptions));
			memberInfos.AddRange(type.GetProperties(publicBindingOptions));

			while (type != null)
			{
				const BindingFlags nonPublicBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic;
				memberInfos.AddRange(type.GetFields(nonPublicBindingOptions));
				memberInfos.AddRange(type.GetProperties(nonPublicBindingOptions));
				type = type.BaseType;
			}

			Type pageObjectType = pageObject.GetType();
			foreach (MemberInfo member in memberInfos)
			{
				LocateAttribute locateAttribute = GetAttribute<LocateAttribute>(member);
				ClickBehaviourAttribute clickBehaviourAttribute = GetAttribute<ClickBehaviourAttribute>(member);
				ClickBehaviours clickBehaviour = clickBehaviourAttribute != null
					                                    ? clickBehaviourAttribute.Using
					                                    : ClickBehaviours.Default;

				FillBehaviourAttribute fillBehaviourAttribute = GetAttribute<FillBehaviourAttribute>(member);
				FillBehaviour fillBehaviour = fillBehaviourAttribute != null 
												? fillBehaviourAttribute.Using 
												: FillBehaviour.Default;
				
				WaitAttribute waitAttribute = GetAttribute<WaitAttribute>(member);
				WaitForElementsOnActionAttribute waitForElementsOnActionAttribute = GetAttribute<WaitForElementsOnActionAttribute>(member);

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

				if (locateAttribute != null)
				{
					By by = CreateLocator(member);
					object createdElementObject;
					FieldInfo field = member as FieldInfo;
					PropertyInfo property = member as PropertyInfo;
					if (field != null)
					{
						createdElementObject = CreateElementObject(field.FieldType, driver, by, waitModel, pageObject, clickBehaviour, fillBehaviour);
						if (createdElementObject == null)
						{
							throw new ArgumentException("Type of field '" + field.Name + "' is not IWebElement or IList<IWebElement>");
						}

						field.SetValue(pageObject, createdElementObject);
					}
					else if (property != null)
					{
						createdElementObject = CreateElementObject(property.PropertyType, driver, by, waitModel, pageObject, clickBehaviour, fillBehaviour);
						if (createdElementObject == null)
						{
							throw new ArgumentException("Type of property '" + property.Name + "' is not IWebElement or IList<IWebElement>");
						}

						property.SetValue(pageObject, createdElementObject, null);
					}
				}

				PartialPageObjectAttribute partialPageObjectAttribute = GetAttribute<PartialPageObjectAttribute>(member);
				if (partialPageObjectAttribute != null)
				{
					PropertyInfo property = member as PropertyInfo;
					if (property != null)
					{

						MethodInfo castMethod = typeof(PageObjectFactory).GetMethod("CreatePageObject", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(property.PropertyType);
						object createdElementObject = castMethod.Invoke(null, new object[] { driver });
						if (createdElementObject == null)
						{
							throw new ArgumentException("Type of property '" + property.Name + "' is not IWebElement or IList<IWebElement>");
						}

						property.SetValue(pageObject, createdElementObject, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,null,null);
					}
				}
			}
		}

		internal static BasicPageElement CreateElementObject(Type fieldType, IWebDriver webDriver, By @by, WaitModel waitModel, BasePageObject parentPageObject, ClickBehaviours clickBehaviours = ClickBehaviours.Default, FillBehaviour fillBehaviour = FillBehaviour.Default)
		{
			if (fieldType == typeof (Button))
			{
				return new Button(@by, webDriver, waitModel, parentPageObject, clickBehaviours);
			}
			if (fieldType == typeof (Link))
			{
				return new Link(@by, webDriver, waitModel, parentPageObject, clickBehaviours);
			}
			if (fieldType == typeof(Image))
			{
				return new Image(@by, webDriver, waitModel, parentPageObject, clickBehaviours);
			}
			if (fieldType == typeof(BasicPageElement))
			{
				return new BasicPageElement(@by, webDriver, waitModel, parentPageObject);
			}
			if (fieldType == typeof(SelectBox))
			{
				return new SelectBox(@by, webDriver, waitModel, parentPageObject);
			}
			if (fieldType == typeof(CheckBox))
			{
				return new CheckBox(@by, webDriver, waitModel, parentPageObject, clickBehaviours);
			}
			if (fieldType == typeof(Input))
			{
				return new Input(@by, webDriver, waitModel, parentPageObject, clickBehaviours, fillBehaviour);
			}
			if (fieldType == typeof(HiddenElement))
			{
				return new HiddenElement(@by, webDriver, waitModel, parentPageObject, clickBehaviours);
			}
			return null;
		}

		public static T GetAttribute<T>(MemberInfo member) where T : Attribute
		{
			Attribute[] attributes = Attribute.GetCustomAttributes(member, typeof(T), true);
			if (attributes.Length > 1)
				throw new ArgumentException("Member " + member.Name + " has " + attributes.Length + " Attributes instead of only one.");
			if (attributes.Length == 0)
				return null;

			Attribute attribute = attributes[0];

			return (T) attribute;

		}
		
		private static By CreateLocator(MemberInfo member)
		{
			LocateAttribute castedAttribute = GetAttribute<LocateAttribute>(member);
			if (castedAttribute.Using == null)
			{
				castedAttribute.Using = member.Name;

			}

			return castedAttribute.Finder;
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
