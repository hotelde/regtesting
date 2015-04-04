using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using RegTesting.Contracts;

namespace RegTesting
{
	/// <summary>
	/// Loading Types of this project.
	/// </summary>
	public class TypesLoader : MarshalByRefObject, ITypesLoader
	{
		#region Implementation of ITypesLoader

		/// <summary>
		/// Get the Types of the Tests Assembly
		/// </summary>
		/// <returns>A string array with all types of the assembly</returns>
		public string[] GetTypes()
		{
			try
			{

				//Find out this assembly.
				Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Single(objAssemb => objAssemb.GetName().Name == "RegTesting.Tests");
				//Get and return all types of this assembly
				IEnumerable<string> types = assembly.GetTypes().Where(t=>typeof(ITestable).IsAssignableFrom(t) && !t.IsAbstract).Select(objType => objType.FullName);
				return types.ToArray();
			}
			catch (InvalidOperationException invalidOperationException)
			{
				//Would be a strange error, but instead of crashing keep it running and report no testcases:
				Console.Error.WriteLine(invalidOperationException);
				return new string[0];
			}
			catch (ReflectionTypeLoadException reflectionTypeLoadException)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Exception loaderException in reflectionTypeLoadException.LoaderExceptions)
				{
					stringBuilder.AppendLine(loaderException.Message);
					if (loaderException is FileNotFoundException)
					{
						FileNotFoundException exFileNotFound = loaderException as FileNotFoundException;
						if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
						{
							stringBuilder.AppendLine("Fusion Log:");
							stringBuilder.AppendLine(exFileNotFound.FusionLog);
						}
					}
					stringBuilder.AppendLine();
				}



				string strErrorMessage = stringBuilder.ToString();
				//Display or log the error based on your application.
				return new [] {"ERROR:" +  strErrorMessage };
			}
		}

		#endregion
	}
}
