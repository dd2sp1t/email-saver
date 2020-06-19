using Unity;
using Unity.Lifetime;
using EmailSaver.Core;

namespace EmailSaver.Client
{
	public static class ContainerHelper
	{
		public static IUnityContainer Container = new UnityContainer();

		static ContainerHelper()
		{
			Container.RegisterType<IEmailSupplier, EmailSupplierHttp>(new ContainerControlledLifetimeManager());
		}
	}
}