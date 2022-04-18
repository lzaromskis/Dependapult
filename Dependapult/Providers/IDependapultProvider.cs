// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace Dependapult.Providers
{
    internal interface IDependapultProvider
    {
        DependapultService GetDependapultService();
    }
}
