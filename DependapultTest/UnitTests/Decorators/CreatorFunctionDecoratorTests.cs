// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using Dependapult;
using Dependapult.Decorators;
using Dependapult.Exceptions;
using DependapultTest.UnitTests.TestUtil;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests.Decorators
{
    public class CreatorFunctionDecoratorTests
    {
        [Fact]
        public void DecorateCreatorFunction_DecoratesFunction()
        {
            var dependapultProvider = Utils.MockDependapultProviderWithoutService();

            CreatorFunctionDecorator decorator = new(dependapultProvider.Object);
            var result = decorator.DecorateCreatorFunction<object>(service => new());

            result.ShouldBeOfType<Func<object>>();
            dependapultProvider.VerifyAll();
        }

        [Fact]
        public void DecorateCreatorFunction_DecoratedFunctionReturnsObject()
        {
            var dependapultProvider = Utils.MockDependapultProvider(out var container);

            CreatorFunctionDecorator decorator = new(dependapultProvider.Object);
            var result = decorator.DecorateCreatorFunction<object>(service => new());

            result().ShouldBeOfType<object>();
            container.VerifyAll();
            dependapultProvider.VerifyAll();
        }

        [Fact]
        public void DecorateCreatorFunction_DecoratedFunctionThrowsExceptionWhenReceivedNull()
        {
            var dependapultProvider = Utils.MockDependapultProvider(out var container);

            CreatorFunctionDecorator decorator = new(dependapultProvider.Object);
#pragma warning disable CS8603 // Possible null reference return.
            var result = decorator.DecorateCreatorFunction<object>(service => null);
#pragma warning restore CS8603 // Possible null reference return.

            Should.Throw<CouldNotCreateObjectOfTypeException>(result);
            container.VerifyAll();
            dependapultProvider.VerifyAll();
        }

        [Fact]
        public void DecorateCreatorFunction_DecoratedFunctionPassesDependapultService()
        {
            var dependapultProvider = Utils.MockDependapultProvider(out var container);

            CreatorFunctionDecorator decorator = new(dependapultProvider.Object);
            var result = decorator.DecorateCreatorFunction<object>(service => service);

            result().ShouldBeOfType<DependapultService>();
            container.VerifyAll();
            dependapultProvider.VerifyAll();
        }
    }
}
