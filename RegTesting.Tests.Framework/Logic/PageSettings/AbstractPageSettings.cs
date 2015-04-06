namespace RegTesting.Tests.Framework.Logic.PageSettings
{
	public abstract class AbstractPageSettings : IPageSettings
	{
		protected BasePageObject PageObject;
		private bool _hasEndlessJQueryAnimation;

		public bool PageUsesJquery { get; set; }

		public bool IsSeoRoute { get; set; }

		public bool HasEndlessJQueryAnimation
		{
			get { return PageUsesJquery && _hasEndlessJQueryAnimation; }
			set { _hasEndlessJQueryAnimation = value; }
		}

		protected AbstractPageSettings(BasePageObject pageObject)
		{
			PageObject = pageObject;
			PageUsesJquery = false;
			HasEndlessJQueryAnimation = false;
		}

		public virtual void ApplySettings()
		{
			TestLog.AddWithoutTime("No settings defined for " + PageObject.GetType().Name);
		}
	}
}
