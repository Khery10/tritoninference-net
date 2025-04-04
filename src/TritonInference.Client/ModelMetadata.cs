namespace TritonInference.Client;

public record ModelMetadata(
    string Name,
    IReadOnlyList<string> Versions,
    string Platform,
    IReadOnlyList<TensorMetadata> Inputs,
    IReadOnlyList<TensorMetadata> Outputs)
{
    public string Name { get; } = Name;
    public IReadOnlyList<string> Versions { get; } = Versions;
    public string Platform { get; } = Platform;
    public IReadOnlyList<TensorMetadata> Inputs { get; } = Inputs;
    public IReadOnlyList<TensorMetadata> Outputs { get; } = Outputs;
}