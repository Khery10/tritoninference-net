using Grpc.Net.Client;
using Inference;

namespace TritonInference.Client.Grpc;

public class TritonInferenceGrpcClient(GRPCInferenceService.GRPCInferenceServiceClient client) : ITritonInferenceClient
{
    public TritonInferenceGrpcClient(string url)
        : this(new GRPCInferenceService.GRPCInferenceServiceClient(GrpcChannel.ForAddress(url)))
    {
    }

    public async Task<bool> IsServerLive()
        => (await client.ServerLiveAsync(new ServerLiveRequest())).Live;

    public async Task<bool> IsServerReady()
        => (await client.ServerReadyAsync(new ServerReadyRequest())).Ready;

    public async Task<bool> IsModelReady(string name, string version)
    {
        var response = await client.ModelReadyAsync(
            new ModelReadyRequest
            {
                Name = name,
                Version = version
            });

        return response.Ready;
    }

    public async Task<TritonInferenceServerMetadata> GetServerMetadata()
    {
        var response = await client.ServerMetadataAsync(new ServerMetadataRequest());
        return new TritonInferenceServerMetadata(
            response.Name,
            response.Version,
            response.Extensions);
    }

    public async Task<ModelMetadata> GetModelMetadata(string name, string version)
    {
        var response = await client.ModelMetadataAsync(
            new ModelMetadataRequest
            {
                Name = name,
                Version = version
            });

        return response.MapToModelMetadata();
    }

    public async Task<IReadOnlyList<RepositoryIndexResponse.Types.ModelIndex>> RepositoryIndex()
    {
        var response = await client.RepositoryIndexAsync(new RepositoryIndexRequest { Ready = false });
        return response.Models;
    }
}