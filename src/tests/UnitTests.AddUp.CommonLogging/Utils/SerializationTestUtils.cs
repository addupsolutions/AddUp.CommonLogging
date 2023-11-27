using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AddUp.CommonLogging.Utils;

// BinayFormatter tests
#pragma warning disable SYSLIB0011 // Type or member is obsolete

[ExcludeFromCodeCoverage]
internal static class SerializationTestUtils
{
    public static void TrySerialization(object o)
    {
        using var stream = new MemoryStream();
        var bformatter = new BinaryFormatter();
        bformatter.Serialize(stream, o);
    }

    public static bool IsSerializable(object o) => o == null || o.GetType().IsSerializable;

    public static bool IsSerializable<T>() => typeof(T).IsSerializable;

    public static T SerializeAndDeserialize<T>(T o)
    {
        using var stream = new MemoryStream();
        var bformatter = new BinaryFormatter();
        bformatter.Serialize(stream, o);
        stream.Flush();

        _ = stream.Seek(0, SeekOrigin.Begin);
        return (T)bformatter.Deserialize(stream);
    }
}

#pragma warning restore SYSLIB0011 // Type or member is obsolete
