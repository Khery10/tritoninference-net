namespace TritonInference.Client;

public record ModelMetadata(
    string Name,
    IReadOnlyList<string> Versions,
    string Platform,
    IReadOnlyList<TensorMetadata> Inputs,
    IReadOnlyList<TensorMetadata> Outputs);