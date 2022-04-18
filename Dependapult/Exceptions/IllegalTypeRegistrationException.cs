// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

namespace Dependapult.Exceptions
{
    /// <summary>
    /// Exception which is thrown when the service received an illegal type registration.
    /// </summary>
    public class IllegalTypeRegistrationException : DependapultException
    {
        /// <summary>
        /// Initializes a new instance of IllegalTypeRegistration class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public IllegalTypeRegistrationException(string message) : base(message)
        {
        }
    }
}
