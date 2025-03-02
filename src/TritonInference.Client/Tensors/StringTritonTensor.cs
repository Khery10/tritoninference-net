using System.Buffers.Binary;
using System.Text;
using TritonInference.Binary;

namespace TritonInference.Client.Tensors;

public class StringTritonTensor : BaseTritonTensor<string>
{
    public StringTritonTensor(
        string name,
        IReadOnlyList<long> shape,
        IReadOnlyList<string> data)
        : base(name, shape, TensorDataTypes.Bytes, data, Array.Empty<byte>())
    {
    }

    public StringTritonTensor(
        string name,
        IReadOnlyList<long> shape,
        ReadOnlyMemory<byte> rawData)
        : base(name, shape, TensorDataTypes.Bytes, Array.Empty<string>(), rawData)
    {
    }

    protected override ReadOnlySpan<byte> SerializeTensor(IReadOnlyList<string> data)
    {
        const int uintSize = sizeof(uint);

        if (data.Count == 0)
            return Array.Empty<byte>();

        uint bytesLen = 0;
        var strLengths = new uint[data.Count];
        for (var i = 0; i < data.Count; i++)
        {
            strLengths[i] = (uint) data[i].GetUtf8ByteCount();
            bytesLen += strLengths[i] + uintSize;
        }

        var bytes = new byte[bytesLen];
        var destination = bytes.AsSpan();
        var offset = 0;

        for (var i = 0; i < data.Count; i++)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(destination[offset..], strLengths[i]);
            offset += uintSize;

            offset += Encoding.UTF8.GetBytes(data[i], destination[offset..]);
        }

        return bytes;
    }

    protected override IReadOnlyList<string> DeserializeTensor(ReadOnlyMemory<byte> rawData)
    {
        if (rawData.Length == 0)
            return [];

        var result = new List<string>();
        var offset = 0;

        while (offset < rawData.Length)
        {
            var len = (int) BinaryPrimitives.ReadUInt32LittleEndian(rawData.Span[offset..]);
            offset += sizeof(uint);

            result.Add(Encoding.UTF8.GetString(rawData.Span.Slice(offset, len)));
            offset += len;
        }

        return result;
    }
}