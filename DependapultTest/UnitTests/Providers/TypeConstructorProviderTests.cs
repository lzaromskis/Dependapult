// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using System.Reflection;
using Dependapult.Exceptions;
using Dependapult.Providers;
using DependapultTest.UnitTests.TestUtil;
using Moq;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests.Providers
{
    public class TypeConstructorProviderTests
    {
        [Fact]
        public void GetConstructor_DefaultConstructor()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneDefaultConstructor);

            var result = provider.GetConstructor(type);

            result.ShouldNotBeNull();
            result.DeclaringType.ShouldBe(type);
            result.GetParameters().ShouldBeEmpty();
        }

        [Fact]
        public void GetConstructor_SingleConstructor_NoParams()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneSingleConstructor);

            var result = provider.GetConstructor(type);

            result.ShouldNotBeNull();
            result.DeclaringType.ShouldBe(type);
            result.GetParameters().ShouldBeEmpty();
        }

        [Fact]
        public void GetConstructor_SingleConstructor_WithOneParam()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneSingleConstructorWithSingleParam);

            var result = provider.GetConstructor(type);

            result.ShouldNotBeNull();
            result.DeclaringType.ShouldBe(type);
            var parameters = result.GetParameters();
            parameters.ShouldNotBeEmpty();
            parameters.Length.ShouldBe(1);
            parameters[0].ParameterType.ShouldBe(typeof(ITestTwo));
        }

        [Fact]
        public void GetConstructor_SingleConstructor_WithTwoParams()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneSingleConstructorWithTwoParams);

            var result = provider.GetConstructor(type);

            result.ShouldNotBeNull();
            result.DeclaringType.ShouldBe(type);
            var parameters = result.GetParameters();
            parameters.ShouldNotBeEmpty();
            parameters.Length.ShouldBe(2);
            parameters[0].ParameterType.ShouldBe(typeof(ITestTwo));
            parameters[1].ParameterType.ShouldBe(typeof(ITestThree));
        }

        [Fact]
        public void GetConstructor_MultipleConstructors_NoAttribute()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneMultipleConstructorsNoTag);

            Should.Throw<CouldNotCreateObjectOfTypeException>(() => provider.GetConstructor(type));
        }

        [Fact]
        public void GetConstructor_MultipleConstructors_OneAttribute()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneMultipleConstructorsOneTag);

            var result = provider.GetConstructor(type);

            result.ShouldNotBeNull();
            result.DeclaringType.ShouldBe(type);
            result.GetParameters().ShouldBeEmpty();
        }

        [Fact]
        public void GetConstructor_MultipleConstructors_OnePublic_OnePrivate()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneMultipleConstructorsOnePublicOnePrivateNoTag);

            var result = provider.GetConstructor(type);

            result.ShouldNotBeNull();
            result.DeclaringType.ShouldBe(type);
            var parameters = result.GetParameters();
            parameters.ShouldNotBeEmpty();
            parameters.Length.ShouldBe(1);
            parameters[0].ParameterType.ShouldBe(typeof(ITestTwo));
        }

        [Fact]
        public void GetConstructor_MultipleConstructors_TwoAttributes()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOneMultipleConstructorsMultipleTags);

            Should.Throw<CouldNotCreateObjectOfTypeException>(() => provider.GetConstructor(type));
        }

        [Fact]
        public void GetConstructor_PrivateConstructor()
        {
            TypeConstructorProvider provider = new();
            Type type = typeof(TestOnePrivateConstructor);

            Should.Throw<CouldNotCreateObjectOfTypeException>(() => provider.GetConstructor(type));
        }

        [Fact]
        public void GetConstructor_ExceptionGettingConstructors()
        {
            TypeConstructorProvider provider = new();
            Mock<Type> type = new();
            type.Setup(v => v.GetConstructors(It.IsAny<BindingFlags>())).Throws(new Exception());

            Should.Throw<CouldNotCreateObjectOfTypeException>(() => provider.GetConstructor(type.Object));
            type.VerifyAll();
        }
    }
}
