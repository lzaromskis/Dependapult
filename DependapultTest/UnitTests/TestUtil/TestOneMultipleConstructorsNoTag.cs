// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace DependapultTest.UnitTests.TestUtil
{
    public class TestOneMultipleConstructorsNoTag : ITestOne
    {
        public TestOneMultipleConstructorsNoTag() { }
        public TestOneMultipleConstructorsNoTag(ITestTwo _) { }
    }
}
