// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using System.Linq;
using System.Reflection;
using Dependapult;
using Dependapult.Decorators;
using Dependapult.Providers;
using Moq;

namespace DependapultTest.UnitTests.TestUtil
{
    internal class Utils
    {
        public static Mock<IDependapultProvider> MockDependapultProvider(out Mock<IContainer> container)
        {
            container = new(MockBehavior.Strict);
            return MockDependapultProvider(container);
        }

        public static Mock<IDependapultProvider> MockDependapultProvider(Mock<IContainer> container)
        {
            Mock<IDependapultProvider> dependapultProvider = new(MockBehavior.Strict);
            dependapultProvider.Setup(v => v.GetDependapultService()).Returns(new DependapultService(container.Object));
            return dependapultProvider;
        }

        public static Mock<IDependapultProvider> MockDependapultProviderWithoutService()
        {
            Mock<IDependapultProvider> dependapultProvider = new(MockBehavior.Strict);
            return dependapultProvider;
        }

        public static Mock<ITypeContructorProvider> MockTypeConstructorProvider(Type type, ConstructorInfo constructor)
        {
            Mock<ITypeContructorProvider> typeContructorProvider = new(MockBehavior.Strict);
            typeContructorProvider.Setup(v => v.GetConstructor(type)).Returns(constructor);
            return typeContructorProvider;
        }

        public static Mock<ICreatorFunctionDecorator> MockCreatorFunctionDecorator(Func<DependapultService, object> creatorFunc)
        {
            Mock<ICreatorFunctionDecorator> decorator = new(MockBehavior.Strict);
            object returnFunc() => creatorFunc(new());
            decorator.Setup(v => v.DecorateCreatorFunction(creatorFunc)).Returns(returnFunc);
            return decorator;
        }

        public static ConstructorInfo GetConstructor(Type type)
        {
            return type.GetConstructors().First();
        }
    }
}
