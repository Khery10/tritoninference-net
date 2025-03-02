using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TritonInference.Client.Tensors;

public class PrimitivesTritonTensor<TDataType> : BaseTritonTensor<TDataType>
    where TDataType : struct
{
    public PrimitivesTritonTensor(
        string name,
        IReadOnlyList<long> shape,
        TensorDataType<TDataType> dataType,
        IReadOnlyList<TDataType> data
    )
        : base(name, shape, dataType, data, Array.Empty<byte>())
    {
        ThrowIfNotSupportedDataType(dataType);
    }

    public PrimitivesTritonTensor(
        string name,
        IReadOnlyList<long> shape,
        TensorDataType<TDataType> dataType,
        ReadOnlyMemory<byte> rawData
    )
        : base(name, shape, dataType, Array.Empty<TDataType>(), rawData)
    {
        ThrowIfNotSupportedDataType(dataType);
    }

    protected override ReadOnlySpan<byte> SerializeTensor(IReadOnlyList<TDataType> data)
    {
        if (data.Count == 0)
            return Array.Empty<byte>();

        if (data is TDataType[] arrayData)
            return MemoryMarshal.Cast<TDataType, byte>(arrayData.AsSpan());

        var size = DataType.NumBytes;
        var bytes = new byte[data.Count * DataType.NumBytes];
        var offset = 0;

        foreach (var item in data)
        {
            var destination = bytes.AsSpan().Slice(offset, size);
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), item);

            offset += size;
        }

        return bytes;
    }

    protected override IReadOnlyList<TDataType> DeserializeTensor(ReadOnlyMemory<byte> rawData)
    {
        return rawData.Length == 0
            ? Array.Empty<TDataType>()
            : MemoryMarshal.Cast<byte, TDataType>(rawData.Span).ToArray();
    }

    private static void ThrowIfNotSupportedDataType(TensorDataType<TDataType> dataType)
    {
        if (dataType.Equals(TensorDataTypes.Bf16))
            throw new ArgumentException($"Data type {dataType.TypeName} not supported");
    }
}