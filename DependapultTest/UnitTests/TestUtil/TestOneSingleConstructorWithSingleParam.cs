// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace DependapultTest.UnitTests.TestUtil
{
    public class TestOneSingleConstructorWithSingleParam : ITestOne
    {
        public ITestTwo Dependency { get; private set; }

        public TestOneSingleConstructorWithSingleParam(ITestTwo dep)
        {
            Dependency = dep ?? throw new ArgumentNullException(nameof(dep));
        }
    }
}
