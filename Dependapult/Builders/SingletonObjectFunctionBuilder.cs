// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using Dependapult.Decorators;
using Dependapult.Providers;

namespace Dependapult.Builders
{
    internal class SingletonObjectFunctionBuilder : TransientObjectFunctionBuilder
    {
        public SingletonObjectFunctionBuilder(IDependapultProvider dependapultProvider,
                                              ITypeContructorProvider typeConstructorProvider,
                                              ICreatorFunctionDecorator creatorFunctionDecorator)
            : base(dependapultProvider, typeConstructorProvider, creatorFunctionDecorator)
        {

        }

        public override Func<object> GetObjectBuildFunction(Type type)
        {
            Lazy<object> instance = new(base.GetObjectBuildFunction(type));
            return () => instance.Value;
        }

        public override Func<object> GetObjectBuildFunction(Type type, object[] args)
        {
            Lazy<object> instance = new(base.GetObjectBuildFunction(type, args));
            return () => instance.Value;
        }

        public override Func<object> GetObjectBuildFunction<TIDep>(Func<DependapultService, TIDep> creatorFunction)
        {
            Lazy<object> instance = new(base.GetObjectBuildFunction<TIDep>(creatorFunction));
            return () => instance.Value;
        }
    }
}
