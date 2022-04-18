// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace Dependapult
{
    /// <summary>
    /// Lifetime of a dependency object.
    /// </summary>
    public enum DependencyLifetime
    {
        /// <summary>
        /// A new object is created only on first request. All other requests return the same object reference.
        /// </summary>
        Singleton,
        /// <summary>
        /// A new object is created every request.
        /// </summary>
        Transient,
    }
}
