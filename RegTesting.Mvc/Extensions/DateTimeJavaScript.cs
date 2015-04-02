using System;

namespace RegTesting.Mvc.Extensions
{
	/// <summary>
	/// A class for DateTime Extensions
	/// </summary>
	public static class DateTimeJavaScript
	{
		private static readonly long DatetimeMinTimeTicks =
		   (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

		/// <summary>
		/// Return a js compatible long with datetime milliseconds.
		/// </summary>
		/// <param name="dt">Datetime object to convert</param>
		/// <returns>A long with the milliseconds</returns>
		public static long ToJavaScriptMilliseconds(this DateTime dt)
		{
			return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
		}
	}
}