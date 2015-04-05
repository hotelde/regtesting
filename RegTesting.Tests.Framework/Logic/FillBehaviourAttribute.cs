using System;
using System.ComponentModel;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// Fillbehaviour attribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class FillBehaviourAttribute : Attribute
	{
		/// <summary>
		/// Method to determine the type
		/// </summary>
		[DefaultValue(FillBehaviour.Default)]
		public FillBehaviour Using { get; set; }
	}
}
