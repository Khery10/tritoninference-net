namespace TritonInference.Client.Tensors;

public abstract class BaseTritonTensor<TDataType> : ITritonTensor<TDataType>
{
    private readonly IReadOnlyList<TDataType> _data;
    private readonly ReadOnlyMemory<byte> _rawData;
    public string Name { get; }
    public IReadOnlyList<long> Shape { get; }
    public string DataTypeName => DataType.TypeName;
    public TensorDataType<TDataType> DataType { get; }

    protected BaseTritonTensor(
        string name,
        IReadOnlyList<long> shape,
        TensorDataType<TDataType> dataType,
        IReadOnlyList<TDataType> data,
        ReadOnlyMemory<byte> rawData)
    {
        Name = name;
        Shape = shape;
        DataType = dataType;
        _data = data;
        _rawData = rawData;
    }

    public ReadOnlySpan<byte> GetRawData()
    {
        return _rawData.IsEmpty
            ? SerializeTensor(_data)
            : _rawData.Span;
    }

    public IReadOnlyList<TDataType> GetData()
    {
        return _data.Count == 0
            ? DeserializeTensor(_rawData)
            : _data;
    }

    protected abstract ReadOnlySpan<byte> SerializeTensor(IReadOnlyList<TDataType> data);
    protected abstract IReadOnlyList<TDataType> DeserializeTensor(ReadOnlyMemory<byte> rawData);
}