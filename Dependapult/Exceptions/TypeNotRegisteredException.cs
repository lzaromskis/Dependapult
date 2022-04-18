// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace Dependapult.Exceptions
{
    /// <summary>
    /// Exception which is thrown when the service could not find a requested type.
    /// </summary>
    public class TypeNotRegisteredException : DependapultException
    {
        /// <summary>
        /// Initializes a new instance of TypeNotRegisteredException class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public TypeNotRegisteredException(string message) : base(message) { }

    }
}
