using System.Globalization;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RegTesting.Mvc.Extensions
{
	/// <summary>
	/// Extensions for the HtmlHelper
	/// </summary>
	public static class HtmlHelperExtensions
	{

		/// <summary>
		/// Create an Image with an action
		/// </summary>
		/// <param name="html">extension hook</param>
		/// <param name="action">action on click</param>
		/// <param name="routeValues">route</param>
		/// <param name="imagePath">path of the image to display</param>
		/// <param name="alt">alt-text of the image to display</param>
		/// <param name="classname">classname to add to the image</param>
		/// <param name="tooltip">tooltip to add to the link</param>
		/// <returns>fullcreated Htmlcode for Use in a View</returns>
		public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt, string classname, string tooltip)
		{
			UrlHelper objURL = new UrlHelper(html.ViewContext.RequestContext);

			// build the <img> tag
			TagBuilder objImgBuilder = new TagBuilder("img");
			objImgBuilder.MergeAttribute("src", objURL.Content(imagePath));
			objImgBuilder.MergeAttribute("alt", alt);
			objImgBuilder.MergeAttribute("class", classname);
			string objImgHtml = objImgBuilder.ToString(TagRenderMode.SelfClosing);

			

			// build the <a> tag
			TagBuilder anchorBuilder = new TagBuilder("a");
			anchorBuilder.MergeAttribute("href", objURL.Action(action, routeValues));
			anchorBuilder.InnerHtml = objImgHtml; // include the <img> tag inside
			anchorBuilder.MergeAttribute("title", tooltip);
			anchorBuilder.MergeAttribute("class", "tooltip");

			string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(anchorHtml);
		}



		/// <summary>
		/// Create a cell with testid and browserid as attributes. Used for testtable
		/// </summary>
		/// <param name="html">extension hook</param>
		/// <param name="testid">testid for test</param>
		/// <param name="templateid">browserid for template</param>
		/// <param name="testsuite">testsuite to assign results</param>
		/// <param name="innerHtml">innerHtml of cell </param>
		/// <returns>fullcreated Htmlcode for Use in a View</returns>
		public static MvcHtmlString JqueryCell(this HtmlHelper html, int testid, int templateid , string testsuite, string innerHtml)
		{
			// build the <td> tag
			TagBuilder objCellBuilder = new TagBuilder("td");
			objCellBuilder.MergeAttribute("class", "testtablecell testaction");
			objCellBuilder.MergeAttribute("testid", testid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.MergeAttribute("browserid", templateid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.InnerHtml = innerHtml;
			string objCellHtml = objCellBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(objCellHtml);
		}



		/// <summary>
		/// Create a cell with testid and browserid as attributes. Used for testtable
		/// </summary>
		/// <param name="html">extension hook</param>
		/// <param name="testid">testid for test</param>
		/// <param name="templateid">browserid for template</param>
		/// <param name="testsuite">testsuite to assign results</param>
		/// <param name="innerHtml">innerHtml of cell </param>
		/// <returns>fullcreated Htmlcode for Use in a View</returns>
		public static MvcHtmlString JqueryHeaderCell(this HtmlHelper html, int testid, int templateid, string testsuite, string innerHtml)
		{
			// build the <td> tag
			TagBuilder objCellBuilder = new TagBuilder("th");
			objCellBuilder.MergeAttribute("class", "testtableheader testaction");
			objCellBuilder.MergeAttribute("testid", testid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.MergeAttribute("browserid", templateid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.InnerHtml = innerHtml;
			string objCellHtml = objCellBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(objCellHtml);
		}


		/// <summary>
		/// Create a cell with testid and browserid as attributes. Used for testtable
		/// </summary>
		/// <param name="html">extension hook</param>
		/// <param name="testid">testid for test</param>
		/// <param name="browserid">browserid for template</param>
		/// <param name="testerid">tester to assign results</param>
		/// <param name="systemid">systemid for testsystem</param>
		/// <param name="languageid">languageid for language</param>
		/// <param name="innerHtml">innerHtml of cell </param>
		/// <param name="extraclasses">classes to add for the resultcell <example>class1 class2</example> </param>
		/// <returns>fullcreated Htmlcode for Use in a View</returns>
		public static MvcHtmlString ResultCell(this HtmlHelper html, int systemid, int testid, int browserid, int languageid, string innerHtml, string extraclasses = "")
		{
			// build the <td> tag
			TagBuilder objCellBuilder = new TagBuilder("td");
			objCellBuilder.MergeAttribute("id", "res"+testid +"-" + browserid + "-" + languageid);
			if (languageid == -1)
			{
				objCellBuilder.MergeAttribute("class", "testtablecell testaction testactionalllanguages "+ extraclasses);
			}
			else
			{
				objCellBuilder.MergeAttribute("class", "testtablecell testaction testactionlanguage " + extraclasses);
			}
			objCellBuilder.MergeAttribute("testid", testid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.MergeAttribute("browserid", browserid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.MergeAttribute("systemid", systemid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.MergeAttribute("languageid", languageid.ToString(CultureInfo.InvariantCulture));
			objCellBuilder.InnerHtml = innerHtml;
			string objCellHtml = objCellBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(objCellHtml);
		}


		/// <summary>
		/// Add a menulink
		/// </summary>
		/// <param name="htmlHelper">extension hook</param>
		/// <param name="linkText">the linktext</param>
		/// <param name="actionName">the action</param>
		/// <param name="controllerName">the controller</param>
		/// <param name="routeValues">the routeValues </param>
		/// <returns>a menulink item</returns>
		public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues){
			//string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
			string strCurrentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			//if (actionName == currentAction && controllerName == currentController)
			if (controllerName == strCurrentController)
			{
				return htmlHelper.ActionLink(
					linkText,
					actionName,
					controllerName,
					routeValues,
					new
					{
						@class = "current"
					});
			}
			return htmlHelper.ActionLink(linkText, actionName, controllerName,routeValues,null);
		}

	}
}
