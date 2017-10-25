using System;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceFabric.Mocks;

namespace Gtm.WebService.Models.Tests.Services
{
    [TestClass]
    public abstract class ServiceBaseTest<TService>
       where TService : IService
    {
        protected TService Service { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Service = CreateService();
        }

        protected abstract TService CreateService();

        protected StatefulServiceContext CreateServiceContext(string serviceTypeName = "")
        {
            return new StatefulServiceContext(
                   new NodeContext(string.Empty, new NodeId(0, 0), 0, string.Empty, string.Empty),
                   new MockCodePackageActivationContext(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
                   serviceTypeName,
                   new Uri("fabric:/Mock"),
                   null,
                   Guid.NewGuid(),
                   0);
        }
    }
}
