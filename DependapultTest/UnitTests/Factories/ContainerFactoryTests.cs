// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using Dependapult;
using Dependapult.Factories;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests.Factories
{
    public class ContainerFactoryTests
    {
        [Fact]
        public void CreateContainer_ReturnsContainerInstance()
        {
            ContainerFactory factory = new();

            var result = factory.CreateContainer();

            result.ShouldBeOfType<Container>();
        }
    }
}
