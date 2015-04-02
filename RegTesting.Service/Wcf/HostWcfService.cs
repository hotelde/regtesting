using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using RegTesting.Service.Logging;

namespace RegTesting.Service.Wcf
{
	/// <summary>
	/// Host a Wcf Service
	/// </summary>
	/// <typeparam name="TEntity">the ServiceInterface to Host</typeparam>
	public class HostWcfService<TEntity> : IHostService<TEntity> where TEntity : class
	{
		private readonly Uri _objBaseAddress;
		
		/// <summary>
		/// Create a new HostWcfService
		/// </summary>
		/// <param name="objBaseAddress">the baseAddress of the service</param>
		public HostWcfService(Uri objBaseAddress)
		{
			if (objBaseAddress == null)
				throw new ArgumentNullException("objBaseAddress");

			_objBaseAddress = objBaseAddress;
		}

		void IHostService<TEntity>.Init(TEntity objEntity)
		{

			Logger.Log("Init " + objEntity.GetType().Name + "...");
			// Create ServiceHost
			ServiceHost objSelfHost = new ServiceHost(objEntity, _objBaseAddress);

			try
			{
				// Add a service endpoint.
				objSelfHost.AddServiceEndpoint(
					typeof(TEntity),
					new WSHttpBinding("WSHttpBinding_IRegressionsTestingService"), "");

				// Enable metadata exchange.
				ServiceMetadataBehavior objServiceMetadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
				objSelfHost.Description.Behaviors.Add(objServiceMetadataBehavior);
				objSelfHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;

				// Start the service.
				objSelfHost.Open();

				Logger.Log("[OK]");

			}
			catch (CommunicationException objCe)
			{
				Logger.Log("An exception occurred: {0}", objCe.Message);
				objSelfHost.Abort();
			}
			catch (Exception objEx)
			{
				Logger.Log("An exception occurred: {0}", objEx.Message);
				objSelfHost.Abort();
			}
		}
	}
}
