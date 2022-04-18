// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using Dependapult;
using Dependapult.Providers;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests.Providers
{
    public class DependapultProviderTests
    {
        [Fact]
        public void GetDependapultService_ReturnsDependapultService()
        {
            DependapultProvider provider = new();

            var service = provider.GetDependapultService();

            service.ShouldNotBeNull();
            service.ShouldBeOfType<DependapultService>();
        }

        [Fact]
        public void GetDependapultService_ReturnsSameInstance()
        {
            DependapultProvider provider = new();

            var service = provider.GetDependapultService();
            var service2 = provider.GetDependapultService();

            service.ShouldBeSameAs(service2);
        }
    }
}
