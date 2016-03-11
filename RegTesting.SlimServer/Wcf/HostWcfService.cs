using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using RegTesting.SlimServer.Logger;

namespace RegTesting.SlimServer.Wcf
{
	/// <summary>
	/// Host a Wcf Service
	/// </summary>
	/// <typeparam name="TEntity">the ServiceInterface to Host</typeparam>
	public class HostWcfService<TEntity> : IHostService<TEntity> where TEntity : class
	{
		private readonly Uri _baseAddress;
		
		/// <summary>
		/// Create a new HostWcfService
		/// </summary>
		/// <param name="baseAddress">the baseAddress of the service</param>
		public HostWcfService(Uri baseAddress)
		{
			if (baseAddress == null)
				throw new ArgumentNullException("baseAddress");

			_baseAddress = baseAddress;
		}

		void IHostService<TEntity>.Init(TEntity entity)
		{

			Log.AddEntry("Init " + entity.GetType().Name + "...");
			// Create ServiceHost
			ServiceHost selfHost = new ServiceHost(entity, _baseAddress);

			try
			{
				// Add a service endpoint.
				selfHost.AddServiceEndpoint(
					typeof(TEntity),
					new WSHttpBinding("WSHttpBinding_IRegressionsTestingService"), "");

				// Enable metadata exchange.
				ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
				selfHost.Description.Behaviors.Add(serviceMetadataBehavior);
				selfHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;

				// Start the service.
				selfHost.Open();

				Log.AddEntry("[OK]");

			}
			catch (CommunicationException objCe)
			{
				Log.AddEntry("An exception occurred: {0}", objCe.Message);
				selfHost.Abort();
			}
			catch (Exception objEx)
			{
				Log.AddEntry("An exception occurred: {0}", objEx.Message);
				selfHost.Abort();
			}
		}
	}
}
