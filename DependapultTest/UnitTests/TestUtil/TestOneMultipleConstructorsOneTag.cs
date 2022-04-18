// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using Dependapult.Attributes;

namespace DependapultTest.UnitTests.TestUtil
{
    public class TestOneMultipleConstructorsOneTag : ITestOne
    {
        [DependapultConstructor]
        public TestOneMultipleConstructorsOneTag() { }

        public TestOneMultipleConstructorsOneTag(ITestTwo _) { }
    }
}
