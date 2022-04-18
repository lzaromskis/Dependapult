// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace Dependapult
{
    internal interface IContainer
    {
        bool Add(DependencyLifetime lifetime, Type interfaceType, Type implementorType);

        bool Add(DependencyLifetime lifetime, Type interfaceType, Type implementorType, object[] args);

        bool Add<TIDep>(DependencyLifetime lifetime, Type interfaceType, Func<DependapultService, TIDep> creatorFunction);

        bool ContainsType(Type interfaceType);

        object Get(Type interfaceType);

        Func<object> GetCreatorFunction(Type interfaceType);
    }
}
