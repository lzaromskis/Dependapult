// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using Dependapult;
using Dependapult.Exceptions;
using DependapultTest.UnitTests.TestUtil;
using Moq;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests
{
    public class DependapultServiceTests
    {
        private static readonly Type InterfaceType = typeof(ITestOne);
        private static readonly Type ImplementorType = typeof(TestOneSingleConstructor);
        private static readonly object[] Args = Array.Empty<object>();
        private static readonly Func<DependapultService, ITestOne> CreatorFunction = service => new TestOneSingleConstructor();

        [Fact]
        public void Instance_ReturnsDependapultServiceInstance()
        {
            var service = DependapultService.Instance;

            service.ShouldBeOfType<DependapultService>();
        }

        [Fact]
        public void Instance_ReturnsSameInstance()
        {
            var service1 = DependapultService.Instance;
            var service2 = DependapultService.Instance;

            service1.ShouldBeSameAs(service2);
        }

        [Fact]
        public void RegisterDependency_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Transient, false);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Transient, false);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_ReturnsTrueIfReplaces()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Transient, true);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_ThrowsExceptionIfIllegalRegistration()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);

            DependapultService service = new(container.Object);

            Should.Throw<IllegalTypeRegistrationException>(() => service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Singleton, true));
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Args_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType, Args)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Transient, Args, false);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Args_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Transient, Args, false);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Args_ReturnsTrueIfReplaces()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType, Args)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Transient, Args, true);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Args_ThrowsExceptionIfIllegalRegistration()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);

            DependapultService service = new(container.Object);

            Should.Throw<IllegalTypeRegistrationException>(() => service.RegisterDependency<ITestOne, TestOneSingleConstructor>(DependencyLifetime.Singleton, Args, true));
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Creator_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, CreatorFunction)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency(DependencyLifetime.Transient, CreatorFunction, false);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Creator_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency(DependencyLifetime.Transient, CreatorFunction, false);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Creator_ReturnsTrueIfReplaces()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, CreatorFunction)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterDependency(DependencyLifetime.Transient, CreatorFunction, true);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterDependency_Creator_ThrowsExceptionIfIllegalRegistration()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);

            DependapultService service = new(container.Object);

            Should.Throw<IllegalTypeRegistrationException>(() => service.RegisterDependency(DependencyLifetime.Singleton, CreatorFunction, true));
            container.VerifyAll();
        }

        [Fact]
        public void RegisterSingleton_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterSingleton<ITestOne, TestOneSingleConstructor>();

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterSingleton_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterSingleton<ITestOne, TestOneSingleConstructor>();

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterSingleton_Args_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Singleton, InterfaceType, ImplementorType, Args)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterSingleton<ITestOne, TestOneSingleConstructor>(Args);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterSingleton_Args_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterSingleton<ITestOne, TestOneSingleConstructor>(Args);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterSingleton_Creator_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Singleton, InterfaceType, CreatorFunction)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterSingleton(CreatorFunction);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterSingleton_Creator_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterSingleton(CreatorFunction);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient<ITestOne, TestOneSingleConstructor>(false);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient<ITestOne, TestOneSingleConstructor>(false);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_ReturnsTrueIfReplaces()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient<ITestOne, TestOneSingleConstructor>(true);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_Args_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType, Args)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient<ITestOne, TestOneSingleConstructor>(Args, false);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_Args_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient<ITestOne, TestOneSingleConstructor>(Args, false);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_Args_ReturnsTrueIfReplaces()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, ImplementorType, Args)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient<ITestOne, TestOneSingleConstructor>(Args, true);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_Creator_ReturnsTrueIfDependencyAdded()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(false);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, CreatorFunction)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient(CreatorFunction, false);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_Creator_ReturnsFalseIfCannotAdd()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.ContainsType(InterfaceType)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient(CreatorFunction, false);

            result.ShouldBeFalse();
            container.VerifyAll();
        }

        [Fact]
        public void RegisterTransient_Creator_ReturnsTrueIfReplaces()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Add(DependencyLifetime.Transient, InterfaceType, CreatorFunction)).Returns(true);

            DependapultService service = new(container.Object);

            var result = service.RegisterTransient(CreatorFunction, true);

            result.ShouldBeTrue();
            container.VerifyAll();
        }

        [Fact]
        public void GetDependencyGenerics_CallsContainerGet()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Get(InterfaceType)).Returns(null);

            DependapultService service = new(container.Object);

            service.GetDependency<ITestOne>();

            container.VerifyAll();
        }

        [Fact]
        public void GetDependency_CallsContainerGet()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.Get(InterfaceType)).Returns(null);

            DependapultService service = new(container.Object);

            service.GetDependency(InterfaceType);

            container.VerifyAll();
        }

        [Fact]
        public void GetDependencyCreatorFunction_CallsContainerGetCreatorFunction()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.GetCreatorFunction(InterfaceType)).Returns(null);

            DependapultService service = new(container.Object);

            service.GetDependencyCreatorFunction(InterfaceType);

            container.VerifyAll();
        }
    }
}
