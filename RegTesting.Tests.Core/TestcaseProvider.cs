using System;
using System.Collections.Generic;
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
		private AppDomain _testsDomain;

		/// <summary>
		/// File with our testcases
		/// </summary>
		private readonly string _testsFile;
	
		/// <summary>
		/// Create a new TestcaseProvider
		/// </summary>
		/// <param name="file">File with our testcases</param>
		public TestcaseProvider(string file)
		{
			_testsFile = file;
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

			string environmentPath = Environment.CurrentDirectory;
			string cachePath = Path.Combine(environmentPath,"__cache");

			PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
			AppDomainSetup appDomainSetup = new AppDomainSetup
			                          	{
											ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
			                          		ShadowCopyFiles = "true",
			                          		CachePath = cachePath
			                          	};
			_testsDomain = AppDomain.CreateDomain("TestDomain", null, appDomainSetup, permissionSet);

			try
			{
                ITypesLoaderFactory typesLoaderFactory = (ITypesLoaderFactory)_testsDomain.CreateInstance(Assembly.GetExecutingAssembly().FullName, "RegTesting.Tests.Core.TypesLoaderFactory").Unwrap();
				object[] constructArgs = new object[] {};
                ITypesLoader typesLoader = typesLoaderFactory.Create(Assembly.GetExecutingAssembly().FullName, "RegTesting.Tests.Core.TypesLoader", constructArgs);
				Types = typesLoader.GetTypes(_testsFile);
				foreach (string type in Types)
				{
					if (type.StartsWith("ERROR:"))
					{
						Console.WriteLine(type);
					}
				}
				return Types;
			}
			catch (NullReferenceException)
			{
				//No types found for this branch
				return new string[0];
			}
			catch (ReflectionTypeLoadException reflectionTypeLoadException)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Exception exSub in reflectionTypeLoadException.LoaderExceptions)
				{
					stringBuilder.AppendLine(exSub.Message);
					if (exSub is FileNotFoundException)
					{
						FileNotFoundException fileNotFoundException = exSub as FileNotFoundException;
						if (!string.IsNullOrEmpty(fileNotFoundException.FusionLog))
						{
							stringBuilder.AppendLine("Fusion Log:");
							stringBuilder.AppendLine(fileNotFoundException.FusionLog);
						}
					}
					stringBuilder.AppendLine();
				}
				string errorMessage = stringBuilder.ToString();
				Console.WriteLine(errorMessage);
				//Display or log the error based on your application.
				return new string[0];
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				//No types found for this branch
				return new string[0];
			}
		}
		

		/// <summary>
		/// Returns a new created ITestable for a given type
		/// </summary>
		/// <param name="typeName">type of testcase</param>
		/// <returns>a new ITestable or null on error</returns>
		public ITestable GetTestableFromTypeName(String typeName)
		{
			try
			{
                ITestableFactory testableFactory = (ITestableFactory)_testsDomain.CreateInstance(Assembly.GetExecutingAssembly().FullName, "RegTesting.Tests.Core.TestableFactory").Unwrap();
				object[] constructArgs = new object[] { };
				ITestable testable = testableFactory.Create(_testsFile,
					typeName, constructArgs);
				return testable;
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception.ToString());
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
				AppDomain.Unload(_testsDomain);
			}
			catch (Exception exception)
			{
				Console.WriteLine("EXCEPTION while unloading: " + exception);
			}
		}


		private void EnsureFilePathExists()
		{
			string path = Path.GetDirectoryName(_testsFile);
			if (!String.IsNullOrWhiteSpace(path))
				Directory.CreateDirectory(path);
		}


		/// <summary>
		/// Initialize the AppDomain and load the types of the dll
		/// </summary>
		/// <returns>the types of the dll</returns>
		public void CreateAppDomain()
		{

			string currentDirectory = Environment.CurrentDirectory;
			string cachePath = Path.Combine(
				currentDirectory,
				"__cache");

			PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
			AppDomainSetup appDomainSetup = new AppDomainSetup
			{
				ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
				ShadowCopyFiles = "true",
				CachePath = cachePath
			};
			_testsDomain = AppDomain.CreateDomain("TestDomain", null, appDomainSetup, permissionSet);

			try
			{
                ITypesLoaderFactory typesLoaderFactory = (ITypesLoaderFactory)_testsDomain.CreateInstance(Assembly.GetExecutingAssembly().FullName, " RegTesting.Tests.Core.TypesLoaderFactory").Unwrap();
				object[] constructArgs = new object[] { };
                ITypesLoader typesLoader = typesLoaderFactory.Create(Assembly.GetExecutingAssembly().FullName, " RegTesting.Tests.Core.TypesLoader", constructArgs);
				Types = typesLoader.GetTypes(_testsFile);
			}
			catch (NullReferenceException)
			{
			}

		}


	}
}
