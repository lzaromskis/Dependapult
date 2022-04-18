// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using Dependapult.Exceptions;
using Dependapult.Providers;

namespace Dependapult.Decorators
{
    internal class CreatorFunctionDecorator : ICreatorFunctionDecorator
    {
        private readonly IDependapultProvider _dependapultProvider;

        public CreatorFunctionDecorator(IDependapultProvider dependapultProvider)
        {
            _dependapultProvider = dependapultProvider;
        }

        public Func<object> DecorateCreatorFunction<TIDep>(Func<DependapultService, TIDep> creatorFunction)
        {
            return () =>
            {
                var service = _dependapultProvider.GetDependapultService();
                return creatorFunction(service) ?? throw new CouldNotCreateObjectOfTypeException($"Failed to create object of type {typeof(TIDep).FullName}");
            };
        }
    }
}
