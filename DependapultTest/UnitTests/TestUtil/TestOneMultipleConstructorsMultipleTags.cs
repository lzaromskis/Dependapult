// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using Dependapult.Attributes;

namespace DependapultTest.UnitTests.TestUtil
{
    public class TestOneMultipleConstructorsMultipleTags : ITestOne
    {
        [DependapultConstructor]
        public TestOneMultipleConstructorsMultipleTags() { }

        [DependapultConstructor]
        public TestOneMultipleConstructorsMultipleTags(ITestTwo _) { }
    }
}
