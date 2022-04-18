// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using System.Linq;
using System.Reflection;
using Dependapult.Attributes;
using Dependapult.Exceptions;

namespace Dependapult.Providers
{
    internal class TypeConstructorProvider : ITypeContructorProvider
    {
        public ConstructorInfo GetConstructor(Type type)
        {
            try
            {
                var constructors = type.GetConstructors();

                if (constructors.Length == 1)
                    return constructors[0];

                if (constructors.Length == 0)
                    throw new CouldNotCreateObjectOfTypeException($"Could not get any constructors for type {type.FullName}");

                var constructorsWithAttribute = GetConstructorsWithAttribute(constructors);

                if (constructorsWithAttribute.Length == 1)
                    return constructorsWithAttribute[0];

                if (constructorsWithAttribute.Length == 0)
                    throw new CouldNotCreateObjectOfTypeException($"Type {type.FullName} has multiple constuctors but no DependapultConstructor attribute");

                throw new CouldNotCreateObjectOfTypeException($"Type {type.FullName} has multiple DependapultConstructor attributes when it should have only one");
            }
            catch (Exception ex)
            {
                if (ex is CouldNotCreateObjectOfTypeException)
                    throw;
                throw new CouldNotCreateObjectOfTypeException($"Failed to get constructor of type {type.FullName}. See inner exception for more information", ex);
            }
        }

        private static ConstructorInfo[] GetConstructorsWithAttribute(ConstructorInfo[] constructors)
        {
            return constructors
                .Where(c => c.GetCustomAttributes(typeof(DependapultConstructorAttribute), false).Any())
                .ToArray();
        }
    }
}
