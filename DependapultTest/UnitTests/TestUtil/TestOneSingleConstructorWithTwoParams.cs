// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace DependapultTest.UnitTests.TestUtil
{
    public class TestOneSingleConstructorWithTwoParams : ITestOne
    {
        public ITestTwo Dependency { get; private set; }
        public ITestThree OtherDependency { get; private set; }

        public TestOneSingleConstructorWithTwoParams(ITestTwo dep, ITestThree depOther)
        {
            Dependency = dep;
            OtherDependency = depOther;
        }
    }
}
