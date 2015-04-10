using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RegTesting.Tests.Core
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
		public string[] GetTypes(string loadAssembly)
		{
			try
			{
				//Load assembly.
                Assembly assembly = Assembly.LoadFrom(loadAssembly);
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



				string errorMessage = stringBuilder.ToString();
				//Display or log the error based on your application.
				return new [] {"ERROR:" +  errorMessage };
			}
		}

		#endregion
	}
}
