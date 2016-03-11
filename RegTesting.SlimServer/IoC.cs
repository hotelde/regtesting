using RegTesting.Contracts.Services;
using RegTesting.SlimServer.Services;
using StructureMap;

namespace RegTesting.SlimServer {

	/// <summary>
	/// IoC class
	/// </summary>
    public static class IoC {
		/// <summary>
		/// Init StructureMap
		/// </summary>
		/// <returns>An IContainer</returns>
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
							x.For<ISlimServerService>().Use<SlimServerService>();
							x.For<INodeService>().Use<NodeService>();
                        });

            return ObjectFactory.Container;
        }
    }
}