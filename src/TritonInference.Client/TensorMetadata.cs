namespace TritonInference.Client;

public record TensorMetadata(
    string Name,
    string DataType,
    IReadOnlyList<long> Shape)
{
    public string Name { get; } = Name;
    public string DataType { get; } = DataType;
    public IReadOnlyList<long> Shape { get; } = Shape;
}