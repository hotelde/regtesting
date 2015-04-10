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
			UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

			// build the <img> tag
			TagBuilder imgBuilder = new TagBuilder("img");
			imgBuilder.MergeAttribute("src", urlHelper.Content(imagePath));
			imgBuilder.MergeAttribute("alt", alt);
			imgBuilder.MergeAttribute("class", classname);
			string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

			

			// build the <a> tag
			TagBuilder anchorBuilder = new TagBuilder("a");
			anchorBuilder.MergeAttribute("href", urlHelper.Action(action, routeValues));
			anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
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
			TagBuilder cellBuilder = new TagBuilder("td");
			cellBuilder.MergeAttribute("class", "testtablecell testaction");
			cellBuilder.MergeAttribute("testid", testid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.MergeAttribute("browserid", templateid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.InnerHtml = innerHtml;
			string cellHtml = cellBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(cellHtml);
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
			TagBuilder cellBuilder = new TagBuilder("th");
			cellBuilder.MergeAttribute("class", "testtableheader testaction");
			cellBuilder.MergeAttribute("testid", testid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.MergeAttribute("browserid", templateid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.InnerHtml = innerHtml;
			string cellHtml = cellBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(cellHtml);
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
			TagBuilder cellBuilder = new TagBuilder("td");
			cellBuilder.MergeAttribute("id", "res"+testid +"-" + browserid + "-" + languageid);
			if (languageid == -1)
			{
				cellBuilder.MergeAttribute("class", "testtablecell testaction testactionalllanguages "+ extraclasses);
			}
			else
			{
				cellBuilder.MergeAttribute("class", "testtablecell testaction testactionlanguage " + extraclasses);
			}
			cellBuilder.MergeAttribute("testid", testid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.MergeAttribute("browserid", browserid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.MergeAttribute("systemid", systemid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.MergeAttribute("languageid", languageid.ToString(CultureInfo.InvariantCulture));
			cellBuilder.InnerHtml = innerHtml;
			string cellHtml = cellBuilder.ToString(TagRenderMode.Normal);

			return MvcHtmlString.Create(cellHtml);
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
			string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			//if (actionName == currentAction && controllerName == currentController)
			if (controllerName == currentController)
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
