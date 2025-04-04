namespace TritonInference.Client;

public record TritonInferenceServerMetadata(
    string Name,
    string Version,
    IReadOnlyList<string> Extensions)
{
    public string Name { get; } = Name;
    public string Version { get; } = Version;
    public IReadOnlyList<string> Extensions { get; } = Extensions;
}