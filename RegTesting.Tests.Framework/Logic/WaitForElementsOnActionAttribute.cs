using System;
using RegTesting.Tests.Framework.Enums;

namespace RegTesting.Tests.Framework.Logic
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class WaitForElementsOnActionAttribute : Attribute
	{
		public When When { get; set; }
		public string WaitForPageElementWithName { get; set; }
		public Visibility Visibility { get; set; }

		public WaitForElementsOnActionAttribute()
		{
			
		}
		/// <summary>
		/// When a action ( e.g. a click ) on the declaired element occures. We will wait for these elements until they have the desired visibility.
		/// </summary>
		/// <param name="when">Wait before or after the action.</param>
		/// <param name="waitForPageElementWithName">The memberName of the page element. E.g. type 'Availabilitybutton' for pageObject.Availabilitybutton with the Id 'AvailButton'.</param>
		/// <param name="visibility">The desired visibility for the object we wait for.</param>
		internal WaitForElementsOnActionAttribute(When when, string waitForPageElementWithName, Visibility visibility)
		{
			When = when;
			WaitForPageElementWithName = waitForPageElementWithName;
			Visibility = visibility;
		}
	}
}
