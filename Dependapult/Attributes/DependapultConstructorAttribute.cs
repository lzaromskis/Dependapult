// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;

namespace Dependapult.Attributes
{
    /// <summary>
    /// Flags which constructor Dependapult should use when creating an object.<br/>
    /// Only one constructor can have this attribute. Not needed when class has only one public constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class DependapultConstructorAttribute : Attribute
    {
    }
}
