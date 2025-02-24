namespace TritonInference.Client;

public record TritonInferenceServerMetadata(
    string Name,
    string Version,
    IReadOnlyList<string> Extensions);