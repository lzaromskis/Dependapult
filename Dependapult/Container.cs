// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using System.Collections.Generic;
using Dependapult.Builders;
using Dependapult.Exceptions;

namespace Dependapult
{
    internal class Container : IContainer
    {
        private readonly Dictionary<Type, Func<object>> _values;
        private readonly IObjectFunctionBuilder _transientBuilder;
        private readonly IObjectFunctionBuilder _singletonBuilder;

        public Container(IObjectFunctionBuilder transientBuilder, IObjectFunctionBuilder singletonBuilder)
        {
            _values = new();
            _transientBuilder = transientBuilder;
            _singletonBuilder = singletonBuilder;
        }

        public bool Add(DependencyLifetime lifetime, Type interfaceType, Type implementorType)
        {
            _values[interfaceType] = lifetime switch
            {
                DependencyLifetime.Singleton => _singletonBuilder.GetObjectBuildFunction(implementorType),
                DependencyLifetime.Transient => _transientBuilder.GetObjectBuildFunction(implementorType),
                _ => throw new ArgumentException($"IOC container does not support {lifetime} lifetime"),
            };
            return true;
        }

        public bool Add(DependencyLifetime lifetime, Type interfaceType, Type implementorType, object[] args)
        {
            _values[interfaceType] = lifetime switch
            {
                DependencyLifetime.Singleton => _singletonBuilder.GetObjectBuildFunction(implementorType, args),
                DependencyLifetime.Transient => _transientBuilder.GetObjectBuildFunction(implementorType, args),
                _ => throw new ArgumentException($"IOC container does not support {lifetime} lifetime"),
            };
            return true;
        }

        public bool Add<TIDep>(DependencyLifetime lifetime, Type interfaceType, Func<DependapultService, TIDep> creatorFunction)
        {
            _values[interfaceType] = lifetime switch
            {
                DependencyLifetime.Singleton => _singletonBuilder.GetObjectBuildFunction(creatorFunction),
                DependencyLifetime.Transient => _transientBuilder.GetObjectBuildFunction(creatorFunction),
                _ => throw new ArgumentException($"IOC container does not support {lifetime} lifetime"),
            };
            return true;
        }

        public bool ContainsType(Type interfaceType)
        {
            return _values.ContainsKey(interfaceType);
        }

        public object Get(Type interfaceType)
        {
            if (!_values.ContainsKey(interfaceType))
                throw new TypeNotRegisteredException($"Cannot get object of unregistered type {interfaceType.FullName}");

            return _values[interfaceType]();
        }

        public Func<object> GetCreatorFunction(Type interfaceType)
        {
            if (!_values.ContainsKey(interfaceType))
                throw new TypeNotRegisteredException($"Cannot get object of unregistered type {interfaceType.FullName}");

            return _values[interfaceType];

        }
    }
}
