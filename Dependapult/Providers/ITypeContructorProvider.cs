// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using System.Reflection;

namespace Dependapult.Providers
{
    internal interface ITypeContructorProvider
    {
        public ConstructorInfo GetConstructor(Type type);
    }
}
