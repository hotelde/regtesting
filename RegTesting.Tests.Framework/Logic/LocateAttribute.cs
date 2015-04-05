using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RegTesting.Tests.Framework.Elements;

namespace RegTesting.Tests.Framework.Logic
{
	/// <summary>
	/// The locate Attribute describes how to find elements
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class LocateAttribute : Attribute, IComparable
	{
		private By _finder = null;

		/// <summary>
		/// Gets or sets the method used to look up the element
		/// </summary>
		[DefaultValue(How.Id)]
		public How How { get; set; }

		/// <summary>
		/// Gets or sets the value to lookup by (i.e. for How.Name, the actual name to look up)
		/// </summary>
		public string Using { get; set; }

		/// <summary>
		/// Gets or sets an explicit <see cref="By"/> object to find by.
		/// Setting this property takes precedence over setting the How or Using properties.
		/// </summary>
		internal By Finder
		{
			get
			{
				if (this._finder == null)
				{
					this._finder = ByFactory.From(this);
				}

				return this._finder;
			}

			set
			{
				this._finder = (By)value;
			}
		}

		/// <summary>
		/// Determines if two <see cref="LocateAttribute"/> instances are equal.
		/// </summary>
		/// <param name="one">One instance to compare.</param>
		/// <param name="two">The other instance to compare.</param>
		/// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(LocateAttribute one, LocateAttribute two)
		{
			// If both are null, or both are same instance, return true.
			if (object.ReferenceEquals(one, two))
			{
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)one == null) || ((object)two == null))
			{
				return false;
			}

			return one.Equals(two);
		}

		/// <summary>
		/// Determines if two <see cref="LocateAttribute"/> instances are unequal.
		/// </summary>s
		/// <param name="one">One instance to compare.</param>
		/// <param name="two">The other instance to compare.</param>
		/// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(LocateAttribute one, LocateAttribute two)
		{
			return !(one == two);
		}

		/// <summary>
		/// Determines if one <see cref="LocateAttribute"/> instance is greater than another.
		/// </summary>
		/// <param name="one">One instance to compare.</param>
		/// <param name="two">The other instance to compare.</param>
		/// <returns><see langword="true"/> if the first instance is greater than the second; otherwise, <see langword="false"/>.</returns>
		public static bool operator >(LocateAttribute one, LocateAttribute two)
		{
			return one.CompareTo(two) > 0;
		}

		/// <summary>
		/// Determines if one <see cref="LocateAttribute"/> instance is less than another.
		/// </summary>
		/// <param name="one">One instance to compare.</param>
		/// <param name="two">The other instance to compare.</param>
		/// <returns><see langword="true"/> if the first instance is less than the second; otherwise, <see langword="false"/>.</returns>
		public static bool operator <(LocateAttribute one, LocateAttribute two)
		{
			return one.CompareTo(two) < 0;
		}

		/// <summary>
		/// Compare this object to another object
		/// </summary>
		/// <param name="obj">other object</param>
		/// <returns>the difference</returns>
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "Object to compare cannot be null");
			}

			LocateAttribute other = obj as LocateAttribute;
			if (other == null)
			{
				throw new ArgumentException("Object to compare must be a LocateAttribute", "obj");
			}
			return 0;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object">Object</see> is equal 
		/// to the current <see cref="System.Object">Object</see>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object">Object</see> to compare with the 
		/// current <see cref="System.Object">Object</see>.</param>
		/// <returns><see langword="true"/> if the specified <see cref="System.Object">Object</see>
		/// is equal to the current <see cref="System.Object">Object</see>; otherwise,
		/// <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			LocateAttribute other = obj as LocateAttribute;
			if (other == null)
			{
				return false;
			}

			if (other.Finder != this.Finder)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="System.Object">Object</see>.</returns>
		public override int GetHashCode()
		{
			return this.Finder.GetHashCode();
		}
	}
}