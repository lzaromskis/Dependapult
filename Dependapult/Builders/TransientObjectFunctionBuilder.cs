// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using System.Linq;
using System.Reflection;
using Dependapult.Decorators;
using Dependapult.Exceptions;
using Dependapult.Providers;

namespace Dependapult.Builders
{
    internal class TransientObjectFunctionBuilder : IObjectFunctionBuilder
    {
        protected readonly IDependapultProvider _dependapultProvider;
        protected readonly ITypeContructorProvider _typeContructorProvider;
        private readonly ICreatorFunctionDecorator _creatorFunctionDecorator;

        public TransientObjectFunctionBuilder(IDependapultProvider dependapultProvider,
                                              ITypeContructorProvider typeContructorProvider,
                                              ICreatorFunctionDecorator creatorFunctionDecorator)
        {
            _dependapultProvider = dependapultProvider;
            _typeContructorProvider = typeContructorProvider;
            _creatorFunctionDecorator = creatorFunctionDecorator;
        }

        public virtual Func<object> GetObjectBuildFunction(Type type)
        {
            var constructor = _typeContructorProvider.GetConstructor(type);
            var dependapultService = _dependapultProvider.GetDependapultService();
            var constructorArgs = GetConstructorArguments(constructor, dependapultService);
            return GetInvokeConstructorFunction(constructor, WrapArguments(constructorArgs));
        }

        public virtual Func<object> GetObjectBuildFunction(Type type, object[] args)
        {
            var constructor = _typeContructorProvider.GetConstructor(type);
            return GetInvokeConstructorFunction(constructor, WrapArguments(args));
        }

        public virtual Func<object> GetObjectBuildFunction<TIDep>(Func<DependapultService, TIDep> creatorFunction)
        {
            var func = _creatorFunctionDecorator.DecorateCreatorFunction(creatorFunction);
            return WrapFunctionWithTryCatch<TIDep>(func);
        }

        private static Func<object[]> WrapArguments(Func<object>[] args)
        {
            return () => args.Select(p => p()).ToArray();
        }

        private static Func<object[]> WrapArguments(object[] args)
        {
            return () => args;
        }

        private static Func<object>[] GetConstructorArguments(ConstructorInfo constructor, DependapultService dependapultService)
        {
            return constructor.GetParameters()
                .Select(p => dependapultService.GetDependencyCreatorFunction(p.ParameterType))
                .ToArray();
        }

        private static Func<object> GetInvokeConstructorFunction(ConstructorInfo constructor, Func<object[]> parameterProvider)
        {
            return () =>
            {
                try
                {
                    var parameters = parameterProvider();
                    return constructor.Invoke(parameters);
                }
                catch (Exception ex)
                {
                    throw new CouldNotCreateObjectOfTypeException($"Failed to build object of type {constructor.DeclaringType?.FullName}. See inner exception for more information", ex);
                }
            };
        }

        private static Func<object> WrapFunctionWithTryCatch<TIDep>(Func<object> func)
        {
            return () =>
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    throw new CouldNotCreateObjectOfTypeException($"Failed to build object of type {typeof(TIDep)}. See inner exception for more information", ex);
                }
            };
        }
    }
}
