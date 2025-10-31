// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// CA1000: Do not declare static members on generic types
[assembly: SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Design decision for utility methods")]

// CA1002: Do not expose generic lists
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Legacy API compatibility")]

// CA1031: Do not catch general exception types
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Intentional catch-all for error handling and logging")]

// CA1062: Validate arguments of public methods
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Parameter validation handled by other means or not required")]

// CA1305: Specify IFormatProvider
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Invariant culture not required in all cases")]

// CA1822: Mark members as static
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Instance members by design")]
