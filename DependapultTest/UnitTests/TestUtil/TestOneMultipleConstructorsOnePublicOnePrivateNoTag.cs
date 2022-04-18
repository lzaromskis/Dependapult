// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace DependapultTest.UnitTests.TestUtil
{
    public class TestOneMultipleConstructorsOnePublicOnePrivateNoTag : ITestOne
    {
        private TestOneMultipleConstructorsOnePublicOnePrivateNoTag() { }
        
        public TestOneMultipleConstructorsOnePublicOnePrivateNoTag(ITestTwo _) { }
    }
}
