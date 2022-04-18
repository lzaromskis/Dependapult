// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace Dependapult.Exceptions
{
    /// <summary>
    /// Base exception type for Dependapult exceptions.
    /// </summary>
    public abstract class DependapultException : Exception
    {
        /// <summary>
        /// Initializes a new instance of DependapultException class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public DependapultException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of DependapultException class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DependapultException(string message, Exception innerException) : base(message, innerException) { }
    }
}
