using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RegTesting.Tests.Framework.Logic
{
	public class SiteParser
	{
		private readonly string _currentWebUrl;

		public SiteParser(string currentWebUrl)
		{
			if (currentWebUrl == null)
				throw new ArgumentNullException(nameof(currentWebUrl));
			_currentWebUrl = currentWebUrl;
		}

		public void ParseCoverageItems(string content)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(content);
			ParseNode(doc.DocumentElement, @"/html[1]");
		}

		private void ParseNode(XmlNode xmlNode, string path = "")
		{
			if (
				xmlNode.Name.Equals("a", StringComparison.InvariantCultureIgnoreCase) ||
				xmlNode.Name.Equals("button", StringComparison.InvariantCultureIgnoreCase) ||
				xmlNode.Attributes?["ng-click"] != null
				)
			{
				TestCoverage.AddEntry(_currentWebUrl,path,"click");
			}

			Dictionary<string,int> elementCounts = new Dictionary<string, int>();
			foreach (XmlNode node in xmlNode.ChildNodes)
			{
				string key = node.Name.ToLower();
				if (!elementCounts.ContainsKey(key))
				{
					elementCounts.Add(key, 1);
				}
				else
				{
					var count = elementCounts[key] + 1;
					elementCounts[key] = count;
				}
				var childPath = path + @"/" + node.Name + "[" +  elementCounts[key] + "]";
				ParseNode(node, childPath);
			}
		}
	}
}
