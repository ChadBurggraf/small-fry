using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Small Fry")]
[assembly: AssemblyDescription("Fluent API design for C# and .NET.")]
[assembly: Guid("82f313d1-d2d2-4a84-b13d-51f4a1f79018")]

#if DEBUG
[assembly: InternalsVisibleTo("SmallFry.Tests")]
#endif