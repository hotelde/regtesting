using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace RegTesting.Tests.Core
{
	/// <summary>
	/// A class that provides testcases from a dll file
	/// </summary>
	public class TestcaseProvider : MarshalByRefObject
	{

		/// <summary>
		///  The app-domain for the tests of one stage
		/// </summary>
		private AppDomain _objTestsDomain;

		/// <summary>
		/// File with our testcases
		/// </summary>
		private readonly string _strTestsFile;
	
		/// <summary>
		/// Create a new TestcaseProvider
		/// </summary>
		/// <param name="strFile">File with our testcases</param>
		public TestcaseProvider(string strFile)
		{
			_strTestsFile = strFile;
			EnsureFilePathExists();
		}

		/// <summary>
		/// The types for a testfile
		/// </summary>
		public string[] Types { get; set; }


		/// <summary>
		/// Initialize the AppDomain and load the types of the dll
		/// </summary>
		/// <returns>the types of the dll</returns>
		public string[] LoadTypes()
		{

			string strEnvironmentPath = Environment.CurrentDirectory;
			string strCachePath = Path.Combine(strEnvironmentPath,"__cache");

			PermissionSet objPermSet = new PermissionSet(PermissionState.Unrestricted);
			AppDomainSetup objSetup = new AppDomainSetup
			                          	{
											ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
			                          		ShadowCopyFiles = "true",
			                          		CachePath = strCachePath
			                          	};
			_objTestsDomain = AppDomain.CreateDomain("TestDomain", null, objSetup, objPermSet);

			try
			{
				ITypesLoaderFactory objTypesLoaderFactory =(ITypesLoaderFactory) _objTestsDomain.CreateInstanceFrom(_strTestsFile, "RegTesting.TypesLoaderFactory").Unwrap();
				object[] objConstructArgs = new object[] {};
				ITypesLoader objTypesLoader = objTypesLoaderFactory.Create(_strTestsFile, "RegTesting.TypesLoader", objConstructArgs);
				Types = objTypesLoader.GetTypes();
				foreach (string strType in Types)
				{
					if (strType.StartsWith("ERROR:"))
					{
						Console.WriteLine(strType);
					}
				}
				return Types;
			}
			catch (NullReferenceException objException)
			{
				//No types found for this branch
				return new string[0];
			}
			catch (ReflectionTypeLoadException objException)
			{
				StringBuilder objStringbuilder = new StringBuilder();
				foreach (Exception objExSub in objException.LoaderExceptions)
				{
					objStringbuilder.AppendLine(objExSub.Message);
					if (objExSub is FileNotFoundException)
					{
						FileNotFoundException exFileNotFound = objExSub as FileNotFoundException;
						if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
						{
							objStringbuilder.AppendLine("Fusion Log:");
							objStringbuilder.AppendLine(exFileNotFound.FusionLog);
						}
					}
					objStringbuilder.AppendLine();
				}
				string strErrorMessage = objStringbuilder.ToString();
				Console.WriteLine(strErrorMessage);
				//Display or log the error based on your application.
				return new string[0];
			}
			catch (Exception objException)
			{
				Console.WriteLine(objException.ToString());
				//No types found for this branch
				return new string[0];
			}
		}
		

		/// <summary>
		/// Returns a new created ITestable for a given type
		/// </summary>
		/// <param name="strTypeName">type of testcase</param>
		/// <returns>a new ITestable or null on error</returns>
		public ITestable GetTestableFromTypeName(String strTypeName)
		{
			try
			{
				ITestableFactory objTestableFactory = (ITestableFactory)_objTestsDomain.CreateInstanceFrom(_strTestsFile, "RegTesting.TestableFactory").Unwrap();
				object[] objConstructArgs = new object[] { };
				ITestable objTestable = objTestableFactory.Create(_strTestsFile,
					strTypeName, objConstructArgs);
				return objTestable;
			}
			catch(Exception objEx)
			{
				Console.WriteLine(objEx.ToString());
				return null;
			}

		}

		/// <summary>
		/// Unload the AppDomain.
		/// </summary>
		public void Unload()
		{
			try
			{

				// release all references to the factory and ILiveInterface
				// unload the complete secondary app-domain
				AppDomain.Unload(_objTestsDomain);
			}
			catch (Exception objEx)
			{
				Console.WriteLine("EXCEPTION while unloading: " + objEx);
			}
		}


		private void EnsureFilePathExists()
		{
			string strPath = Path.GetDirectoryName(_strTestsFile);
			if (!String.IsNullOrWhiteSpace(strPath))
				Directory.CreateDirectory(strPath);
		}


		/// <summary>
		/// Initialize the AppDomain and load the types of the dll
		/// </summary>
		/// <returns>the types of the dll</returns>
		public void CreateAppDomain()
		{

			string strEnvironmentPath = Environment.CurrentDirectory;
			string strCachePath = Path.Combine(
				strEnvironmentPath,
				"__cache");

			PermissionSet objPermSet = new PermissionSet(PermissionState.Unrestricted);
			AppDomainSetup objSetup = new AppDomainSetup
			{
				ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
				ShadowCopyFiles = "true",
				CachePath = strCachePath
			};
			_objTestsDomain = AppDomain.CreateDomain("TestDomain", null, objSetup, objPermSet);


			try
			{
				ITypesLoaderFactory objTypesLoaderFactory = (ITypesLoaderFactory)_objTestsDomain.CreateInstanceFrom(_strTestsFile, "RegTesting.TypesLoaderFactory").Unwrap();
				object[] objConstructArgs = new object[] { };
				ITypesLoader objTypesLoader = objTypesLoaderFactory.Create(_strTestsFile, "RegTesting.TypesLoader", objConstructArgs);
				Types = objTypesLoader.GetTypes();
			}
			catch (NullReferenceException)
			{
			}

		}


	}
}
