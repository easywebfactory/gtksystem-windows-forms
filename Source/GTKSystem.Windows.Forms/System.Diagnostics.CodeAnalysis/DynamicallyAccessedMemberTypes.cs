#if NETSTANDARD2_0
using System;

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies the types of members that are dynamically accessed.
    ///        This enumeration has a <see cref="T:System.FlagsAttribute" /> attribute that allows a bitwise combination of its member values.</summary>
    [Flags]
    internal enum DynamicallyAccessedMemberTypes
    {
        /// <summary>Specifies all members.</summary>
        All = -1,
        /// <summary>Specifies no members.</summary>
        None = 0,
        /// <summary>Specifies the default, parameterless public constructor.</summary>
        PublicParameterlessConstructor = 1,
        /// <summary>Specifies all public constructors.</summary>
        PublicConstructors = 3,
        /// <summary>Specifies all non-public constructors.</summary>
        NonPublicConstructors = 4,
        /// <summary>Specifies all public methods.</summary>
        PublicMethods = 8,
        /// <summary>Specifies all non-public methods.</summary>
        NonPublicMethods = 16,
        /// <summary>Specifies all public fields.</summary>
        PublicFields = 32,
        /// <summary>Specifies all non-public fields.</summary>
        NonPublicFields = 64,
        /// <summary>Specifies all public nested types.</summary>
        PublicNestedTypes = 128,
        /// <summary>Specifies all non-public nested types.</summary>
        NonPublicNestedTypes = 256,
        /// <summary>Specifies all public properties.</summary>
        PublicProperties = 512,
        /// <summary>Specifies all non-public properties.</summary>
        NonPublicProperties = 1024,
        /// <summary>Specifies all public events.</summary>
        PublicEvents = 2048,
        /// <summary>Specifies all non-public events.</summary>
        NonPublicEvents = 4096,
        /// <summary>Specifies all interfaces implemented by the type.</summary>
        Interfaces = 8192
    }
}
#endif