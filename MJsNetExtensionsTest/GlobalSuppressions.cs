// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// CA1002: Do not expose generic lists
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Test classes - API design rules relaxed")]

// CA1031: Do not catch general exception types
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Test code - intentional catch-all for testing error scenarios")]

// CA1051: Do not declare visible instance fields
[assembly: SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Test data classes - fields acceptable for serialization testing")]

// CA1305: Specify IFormatProvider
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Test code - culture-specific formatting not critical")]

// CA1819: Properties should not return arrays
[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Test classes - array properties acceptable for XML serialization testing")]

// CA2201: Do not raise reserved exception types
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Test code - intentionally testing generic exception handling")]

// CA2227: Collection properties should be read only
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Test classes - settable properties required for XML serialization/deserialization testing")]
