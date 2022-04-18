// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace Dependapult.Exceptions
{
    /// <summary>
    /// Exception which is thrown when the service could not create a dependency object.
    /// </summary>
    public class CouldNotCreateObjectOfTypeException : DependapultException
    {
        /// <summary>
        /// Initializes a new instance of CouldNotCreateObjectOfTypeException class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public CouldNotCreateObjectOfTypeException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of CouldNotCreateObjectOfTypeException class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public CouldNotCreateObjectOfTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
