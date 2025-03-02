using System.Buffers.Binary;

namespace TritonInference.Binary;

public static class Bf16Converter
{
    private static readonly byte[] Zeros = [0, 0];

    public static void WriteBf16(float num, Span<byte> destination)
    {
        if (destination.Length < 2)
            throw new ArgumentOutOfRangeException($"Destination length less than 2");

        Span<byte> buf = stackalloc byte[4];
        BitConverter.TryWriteBytes(buf, num);

        buf[2..].CopyTo(destination);
    }

    public static float ReadBf16(ReadOnlySpan<byte> source)
    {
        if (source.Length != 2)
            throw new ArgumentOutOfRangeException($"Source length should be 2, but was {source.Length}");

        Span<byte> buf = stackalloc byte[4];
        Zeros.CopyTo(buf);
        source.CopyTo(buf[2..]);
        
        return BinaryPrimitives.ReadSingleLittleEndian(buf);
    }
}