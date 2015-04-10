namespace RegTesting.Contracts.Enums
{

	/// <summary>
	/// the jobType enum
	/// </summary>
	public enum JobType
	{
		Unspecified		= 0,
		Testportal		= 1, //started in testportal
		Buildtask		= 2, //started automatic from buildtask
		Localtesttool	= 3 //started in localtesttool
	}
}
