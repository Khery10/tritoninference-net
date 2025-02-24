namespace TritonInference.Client;

public record TensorMetadata(
    string Name,
    string DataType,
    IReadOnlyList<long> Shape);