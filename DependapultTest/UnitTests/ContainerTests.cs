// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using Dependapult;
using Dependapult.Builders;
using Dependapult.Exceptions;
using DependapultTest.UnitTests.TestUtil;
using Moq;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests
{
    public class ContainerTests
    {
        private static readonly Type InterfaceType = typeof(ITestOne);
        private static readonly Type ImplementorType = typeof(TestOneSingleConstructor);

        private static readonly TestOneSingleConstructor ImplementorInstance = new();

        [Fact]
        public void Add_AddsSingleton_Interface_Type()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);
            singletonBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType)).Returns(() => ImplementorInstance);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            var result = container.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_AddsTransient_Interface_Type()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            transientBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType)).Returns(() => ImplementorInstance);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            var result = container.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_ThrowsException_Interface_Type()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);

            Should.Throw<ArgumentException>(() => container.Add((DependencyLifetime)int.MaxValue, InterfaceType, ImplementorType));
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_AddsSingleton_Interface_Type_Args()
        {
            var args = Array.Empty<object>();

            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);
            singletonBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType, args)).Returns(() => ImplementorInstance);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            var result = container.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType, args);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_AddsTransient_Interface_Type_Args()
        {
            var args = Array.Empty<object>();

            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            transientBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType, args)).Returns(() => ImplementorInstance);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            var result = container.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType, args);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_ThrowsException_Interface_Type_Args()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);

            Should.Throw<ArgumentException>(() => container.Add((DependencyLifetime)int.MaxValue, InterfaceType, ImplementorType, Array.Empty<object>()));
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_AddsSingleton_Interface_Creator()
        {
            Func<DependapultService, TestOneSingleConstructor> creatorFunc = service => ImplementorInstance;

            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);
            singletonBuilder.Setup(v => v.GetObjectBuildFunction(creatorFunc)).Returns(() => ImplementorInstance);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            var result = container.Add(DependencyLifetime.Singleton, InterfaceType, creatorFunc);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_AddsTransient_Interface_Creator()
        {
            Func<DependapultService, TestOneSingleConstructor> creatorFunc = service => ImplementorInstance;

            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            transientBuilder.Setup(v => v.GetObjectBuildFunction(creatorFunc)).Returns(() => ImplementorInstance);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            var result = container.Add(DependencyLifetime.Transient, InterfaceType, creatorFunc);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Add_ThrowsException_Interface_Creator()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);

            Should.Throw<ArgumentException>(() => container.Add((DependencyLifetime)int.MaxValue, InterfaceType, service => ImplementorInstance));
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void ContainsType_IsFalse()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);

            var result = container.ContainsType(InterfaceType);

            result.ShouldBeFalse();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void ContainsType_IsTrue()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);
            singletonBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType)).Returns(() => ImplementorInstance);


            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            container.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType);

            var result = container.ContainsType(InterfaceType);

            result.ShouldBeTrue();
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Get_ReturnsCreatedInstance()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);
            singletonBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType)).Returns(() => ImplementorInstance);


            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            container.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType);

            var result = container.Get(InterfaceType);

            result.ShouldBeSameAs(ImplementorInstance);
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void Get_ThrowsExceptionIfNotFound()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);

            Should.Throw<TypeNotRegisteredException>(() => container.Get(InterfaceType));
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void GetCreatorFunction_ReturnsCreatedInstance()
        {
            Func<TestOneSingleConstructor> buildFunc = () => ImplementorInstance;

            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);
            singletonBuilder.Setup(v => v.GetObjectBuildFunction(ImplementorType)).Returns(buildFunc);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);
            container.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType);

            var result = container.GetCreatorFunction(InterfaceType);

            result.ShouldBeSameAs(buildFunc);
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }

        [Fact]
        public void GetCreatorFunction_ThrowsExceptionWhenNotFound()
        {
            Mock<IObjectFunctionBuilder> transientBuilder = new(MockBehavior.Strict);
            Mock<IObjectFunctionBuilder> singletonBuilder = new(MockBehavior.Strict);

            Container container = new(transientBuilder.Object, singletonBuilder.Object);

            Should.Throw<TypeNotRegisteredException>(() => container.GetCreatorFunction(InterfaceType));
            transientBuilder.VerifyAll();
            singletonBuilder.VerifyAll();
        }
    }
}
