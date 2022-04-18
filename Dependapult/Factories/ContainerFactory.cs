// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using Dependapult.Builders;
using Dependapult.Decorators;
using Dependapult.Providers;

namespace Dependapult.Factories
{
    internal class ContainerFactory : IContainerFactory
    {
        public IContainer CreateContainer()
        {
            DependapultProvider dependapultProvider = new();
            TypeConstructorProvider typeConstructorProvider = new();
            CreatorFunctionDecorator creatorFunctionDecorator = new(dependapultProvider);

            return new Container(
                new TransientObjectFunctionBuilder(dependapultProvider, typeConstructorProvider, creatorFunctionDecorator),
                new SingletonObjectFunctionBuilder(dependapultProvider, typeConstructorProvider, creatorFunctionDecorator)
            );
        }
    }
}
