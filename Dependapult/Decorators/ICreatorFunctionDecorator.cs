// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace Dependapult.Decorators
{
    internal interface ICreatorFunctionDecorator
    {
        Func<object> DecorateCreatorFunction<TIDep>(Func<DependapultService, TIDep> creatorFunction);
    }
}
