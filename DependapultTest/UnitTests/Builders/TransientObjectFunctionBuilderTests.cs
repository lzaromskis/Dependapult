// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8603 // Possible null reference return.

using System;
using System.Reflection;
using Dependapult;
using Dependapult.Builders;
using Dependapult.Exceptions;
using DependapultTest.UnitTests.TestUtil;
using Moq;
using Shouldly;
using Xunit;

namespace DependapultTest.UnitTests.Builders
{
    public class TransientObjectFunctionBuilderTests
    {
        private static readonly Type ObjectType = typeof(TestOneSingleConstructorWithSingleParam);
        private static readonly Type DependencyType = typeof(ITestTwo);
        private static readonly TestTwo DependencyInstance = new();
        private static readonly ConstructorInfo ObjectConstructor = Utils.GetConstructor(ObjectType);

        [Fact]
        public void GetObjectBuildFunction_Type_ReturnsObjectFunction()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.GetCreatorFunction(DependencyType)).Returns(() => DependencyInstance);
            var dependapultProvider = Utils.MockDependapultProvider(container);
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(dependapultProvider.Object, constructorProvider.Object, null);
            var result = builder.GetObjectBuildFunction(ObjectType);

            result.ShouldBeOfType<Func<object>>();

            container.VerifyAll();
            dependapultProvider.VerifyAll();
            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_ReturnsObjectFunction_CreatesObject()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.GetCreatorFunction(DependencyType)).Returns(() => DependencyInstance);
            var dependapultProvider = Utils.MockDependapultProvider(container);
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(dependapultProvider.Object, constructorProvider.Object, null);
            var result = builder.GetObjectBuildFunction(ObjectType)();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestOneSingleConstructorWithSingleParam>();
            (result as TestOneSingleConstructorWithSingleParam).Dependency.ShouldBeSameAs(DependencyInstance);

            container.VerifyAll();
            dependapultProvider.VerifyAll();
            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_ReturnsObjectFunction_ReturnsDifferentObject()
        {
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.GetCreatorFunction(DependencyType)).Returns(() => DependencyInstance);
            var dependapultProvider = Utils.MockDependapultProvider(container);
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(dependapultProvider.Object, constructorProvider.Object, null);
            var buildFunc = builder.GetObjectBuildFunction(ObjectType);
            var res1 = buildFunc();
            var res2 = buildFunc();

            res1.ShouldNotBeSameAs(res2);

            container.VerifyAll();
            dependapultProvider.VerifyAll();
            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_ThrowsExceptionFailedCreation()
        {
            static object buildFunc() => throw new Exception();
            Mock<IContainer> container = new(MockBehavior.Strict);
            container.Setup(v => v.GetCreatorFunction(DependencyType)).Returns(buildFunc);
            var dependapultProvider = Utils.MockDependapultProvider(container);
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(dependapultProvider.Object, constructorProvider.Object, null);
            var result = builder.GetObjectBuildFunction(ObjectType);

            Should.Throw<CouldNotCreateObjectOfTypeException>(result);

            container.VerifyAll();
            dependapultProvider.VerifyAll();
            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_Args_ReturnsObjectFunction()
        {
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(null, constructorProvider.Object, null);
            var result = builder.GetObjectBuildFunction(ObjectType, new[] { DependencyInstance });

            result.ShouldBeOfType<Func<object>>();

            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_Args_ReturnsObjectFunction_CreatesObject()
        {
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(null, constructorProvider.Object, null);
            var result = builder.GetObjectBuildFunction(ObjectType, new[] { DependencyInstance })();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestOneSingleConstructorWithSingleParam>();
            (result as TestOneSingleConstructorWithSingleParam).Dependency.ShouldBeSameAs(DependencyInstance);

            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_Args_ReturnsObjectFunction_ReturnsDifferentObject()
        {
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(null, constructorProvider.Object, null);
            var buildFunc = builder.GetObjectBuildFunction(ObjectType, new[] { DependencyInstance });
            var res1 = buildFunc();
            var res2 = buildFunc();

            res1.ShouldNotBeSameAs(res2);

            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_Type_Args_ThrowsExceptionFailedCreation()
        {
            var constructorProvider = Utils.MockTypeConstructorProvider(ObjectType, ObjectConstructor);

            TransientObjectFunctionBuilder builder = new(null, constructorProvider.Object, null);
            var result = builder.GetObjectBuildFunction(ObjectType, new[] { null as object });

            Should.Throw<CouldNotCreateObjectOfTypeException>(result);

            constructorProvider.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_CreatorFunc_ReturnsObjectFunction()
        {
            static object creatorFunc(DependapultService service) => null;
            var creatorFunctionDecorator = Utils.MockCreatorFunctionDecorator(creatorFunc);

            TransientObjectFunctionBuilder builder = new(null, null, creatorFunctionDecorator.Object);
            var result = builder.GetObjectBuildFunction(creatorFunc);

            result.ShouldBeOfType<Func<object>>();

            creatorFunctionDecorator.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_CreatorFunc_ReturnsObjectFunction_CreatesObject()
        {
            static object creatorFunc(DependapultService service) => new TestOneSingleConstructorWithSingleParam(DependencyInstance);
            var creatorFunctionDecorator = Utils.MockCreatorFunctionDecorator(creatorFunc);

            TransientObjectFunctionBuilder builder = new(null, null, creatorFunctionDecorator.Object);
            var result = builder.GetObjectBuildFunction(creatorFunc)();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestOneSingleConstructorWithSingleParam>();
            (result as TestOneSingleConstructorWithSingleParam).Dependency.ShouldBeSameAs(DependencyInstance);

            creatorFunctionDecorator.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_CreatorFunc_ReturnsObjectFunction_ReturnsDifferentObject()
        {
            static object creatorFunc(DependapultService service) => new TestOneSingleConstructorWithSingleParam(DependencyInstance);
            var creatorFunctionDecorator = Utils.MockCreatorFunctionDecorator(creatorFunc);

            TransientObjectFunctionBuilder builder = new(null, null, creatorFunctionDecorator.Object);
            var buildFunc = builder.GetObjectBuildFunction(creatorFunc);
            var res1 = buildFunc();
            var res2 = buildFunc();

            res1.ShouldNotBeSameAs(res2);

            creatorFunctionDecorator.VerifyAll();
        }

        [Fact]
        public void GetObjectBuildFunction_CreatorFunc_ThrowsExceptionFailedCreation()
        {
            static object creatorFunc(DependapultService service) => new TestOneSingleConstructorWithSingleParam(null);
            var creatorFunctionDecorator = Utils.MockCreatorFunctionDecorator(creatorFunc);

            TransientObjectFunctionBuilder builder = new(null, null, creatorFunctionDecorator.Object);
            var result = builder.GetObjectBuildFunction(creatorFunc);

            Should.Throw<CouldNotCreateObjectOfTypeException>(result);

            creatorFunctionDecorator.VerifyAll();
        }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning restore CS8603 // Possible null reference return.
