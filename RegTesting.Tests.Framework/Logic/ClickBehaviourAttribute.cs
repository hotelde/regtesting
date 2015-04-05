using System;
using System.ComponentModel;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// The locate Attribute describes how to find elements
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class ClickBehaviourAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the method used to look up the element
		/// </summary>
		[DefaultValue(ClickBehaviours.Default)]
		public ClickBehaviours Using { get; set; }

	}
}