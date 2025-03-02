using TritonInference.Binary;

namespace TritonInference.Client.Tensors;

public class Bf16TritonTensor : BaseTritonTensor<float>
{
    private static readonly int BytesLen = TensorDataTypes.Bf16.NumBytes;

    public Bf16TritonTensor(
        string name,
        IReadOnlyList<long> shape,
        IReadOnlyList<float> data)
        : base(name, shape, TensorDataTypes.Bf16, data, Array.Empty<byte>())
    {
    }

    public Bf16TritonTensor(
        string name,
        IReadOnlyList<long> shape,
        ReadOnlyMemory<byte> rawData)
        : base(name, shape, TensorDataTypes.Bf16, Array.Empty<float>(), rawData)
    {
    }
    
    protected override ReadOnlySpan<byte> SerializeTensor(IReadOnlyList<float> data)
    {
        if (data.Count == 0)
            return Array.Empty<byte>();

        var bytes = new byte[data.Count * BytesLen];
        var span = bytes.AsSpan();

        for (var i = 0; i < data.Count; i++)
        {
            Bf16Converter.WriteBf16(data[i], span);
            span = span[BytesLen..];
        }

        return bytes;
    }

    protected override IReadOnlyList<float> DeserializeTensor(ReadOnlyMemory<byte> rawData)
    {
        if (rawData.Length == 0)
            return Array.Empty<float>();

        var data = new float[rawData.Length / BytesLen];
        var offset = 0;
        var index = 0;

        while (offset < rawData.Length)
        {
            data[index++] = Bf16Converter.ReadBf16(rawData.Span.Slice(offset, BytesLen));
            offset += BytesLen;
        }

        return data;
    }
}