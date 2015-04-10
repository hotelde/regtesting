using System;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using RegTesting.Contracts.Domain;

namespace RegTesting.Service.Tfs
{
	internal static class TfsBuildQuery
	{

		private static DateTime _lastQueryed = DateTime.MinValue;
		private static readonly object Lock = new object();
		private static IQueuedBuild[] _queuedBuilds;

		public static bool IsDeploymentRunning(string deployname)
		{
			lock (Lock)
			{
				//Cache the query results for some seconds...
				if (DateTime.Now - _lastQueryed > new TimeSpan(0, 0, 30) || _queuedBuilds == null)
				{
					QueryQueuedBuilds();
				}
			}
			try
			{
				if (_queuedBuilds.Any(queuedBuild => queuedBuild.Status == QueueStatus.InProgress && queuedBuild.BuildDefinition != null
				                                           && queuedBuild.BuildDefinition.Name != null && queuedBuild.BuildDefinition.Name.ToLower().Equals(deployname.ToLower())))
				{
					return true;
				}
			}
			catch(Exception exception)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Out.WriteLine(exception);
				Console.ResetColor();
			}
			return false;

		}

		public static bool IsDeploymentRunning(Testsystem testsystem)
		{
			return IsDeploymentRunning(GetDeploymentName(testsystem));
		}


		private static void QueryQueuedBuilds()
		{
			TfsTeamProjectCollection tfsTeamProjectCollection =
				TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(RegtestingServerConfiguration.TfsUrl));

			tfsTeamProjectCollection.EnsureAuthenticated();

			IBuildServer buildServer = tfsTeamProjectCollection.GetService<IBuildServer>();
			_queuedBuilds = buildServer.QueryQueuedBuilds(buildServer.CreateBuildQueueSpec("*")).QueuedBuilds;
			_lastQueryed = DateTime.Now;
		}

		public static String GetDeploymentName(Testsystem testsystem)
		{
			return testsystem.Url;

		}
	}

}
