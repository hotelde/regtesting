using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;

namespace RegTesting.Service.Tfs
{
	internal static class TfsBuildQuery
	{

		private static DateTime _datLastQueryed = DateTime.MinValue;
		private static readonly object ObjLock = new object();
		private static IQueuedBuild[] _objQueuedBuilds;

		public static bool IsDeploymentRunning(string strDeployname)
		{
			lock (ObjLock)
			{
				//Cache the query results for some seconds...
				if (DateTime.Now - _datLastQueryed > new TimeSpan(0, 0, 30) || _objQueuedBuilds == null)
				{
					QueryQueuedBuilds();
				}
			}
			try
			{
				if (_objQueuedBuilds.Any(objQueuedBuild => objQueuedBuild.Status == QueueStatus.InProgress && objQueuedBuild.BuildDefinition != null
				                                           && objQueuedBuild.BuildDefinition.Name != null && objQueuedBuild.BuildDefinition.Name.ToLower().Equals(strDeployname.ToLower())))
				{
					return true;
				}
			}
			catch(Exception objException)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Out.WriteLine(objException);
				Console.ResetColor();
			}
			return false;

		}

		public static bool IsDeploymentRunning(Testsystem objTestsystem)
		{
			return IsDeploymentRunning(GetDeploymentName(objTestsystem));
		}


		private static void QueryQueuedBuilds()
		{
			TfsTeamProjectCollection objTfs =
				TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(RegtestingServerConfiguration.TfsUrl));

			objTfs.EnsureAuthenticated();

			IBuildServer objBuildServer = objTfs.GetService<IBuildServer>();
			_objQueuedBuilds = objBuildServer.QueryQueuedBuilds(objBuildServer.CreateBuildQueueSpec("*")).QueuedBuilds;
			_datLastQueryed = DateTime.Now;
		}

		public static String GetDeploymentName(Testsystem objTestsystem)
		{
			return objTestsystem.Url;

		}
	}

}
