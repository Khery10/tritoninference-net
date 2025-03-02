namespace TritonInference.Client.Tensors;

public interface ITritonTensor<TDataType> : ITritonTensor
{
    public TensorDataType<TDataType> DataType { get; }
    public IReadOnlyList<TDataType> GetData();
}

public interface ITritonTensor
{
    public string Name { get; }
    public IReadOnlyList<long> Shape { get; }
    public string DataTypeName { get; }
    public ReadOnlySpan<byte> GetRawData();
}