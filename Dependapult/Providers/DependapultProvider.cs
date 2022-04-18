// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace Dependapult.Providers
{
    internal class DependapultProvider : IDependapultProvider
    {
        public DependapultService GetDependapultService()
        {
            return DependapultService.Instance;
        }
    }
}
