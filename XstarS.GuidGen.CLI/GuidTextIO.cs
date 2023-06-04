using System;
using System.IO;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif

namespace XstarS.GuidGen;

internal static class GuidTextIO
{
    internal static void Write(this TextWriter writer, in Guid value)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        const int textLength = 36;
        var textBuffer = (stackalloc char[textLength]);
        var result = value.TryFormat(textBuffer, out var charsWritten);
        Debug.Assert(result && (charsWritten == textLength));
        writer.Write(textBuffer);
#else
        writer.Write(value.ToString());
#endif
    }

    internal static void WriteLine(this TextWriter writer, in Guid value)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        const int textLength = 36;
        var textBuffer = (stackalloc char[textLength]);
        var result = value.TryFormat(textBuffer, out var charsWritten);
        Debug.Assert(result && (charsWritten == textLength));
        writer.WriteLine(textBuffer);
#else
        writer.WriteLine(value.ToString());
#endif
    }
}
