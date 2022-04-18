// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace Dependapult.Builders
{
    internal interface IObjectFunctionBuilder
    {
        Func<object> GetObjectBuildFunction(Type type);
        Func<object> GetObjectBuildFunction(Type type, object[] args);
        Func<object> GetObjectBuildFunction<TIDep>(Func<DependapultService, TIDep> creatorFunction);
    }
}
