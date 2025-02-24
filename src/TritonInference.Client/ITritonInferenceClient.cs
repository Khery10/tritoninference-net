namespace TritonInference.Client;

public interface ITritonInferenceClient
{
    Task<bool> IsServerLive();
    Task<bool> IsServerReady();
    Task<bool> IsModelReady(string name, string version);
    Task<TritonInferenceServerMetadata> GetServerMetadata();
    Task<ModelMetadata> GetModelMetadata(string name, string version);
}