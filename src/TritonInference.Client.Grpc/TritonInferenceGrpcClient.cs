using Google.Protobuf;
using Grpc.Net.Client;
using Inference;
using TritonInference.Client.Tensors;

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

    public async Task<ITritonInferenceResult> Infer(TritonInferenceRequest request, CancellationToken cancellationToken)
    {
        var modelInferRequest = CreateModelInferRequest(request);
        var response = await client.ModelInferAsync(modelInferRequest, cancellationToken: cancellationToken);

        return new TritonInferenceGrpcResult(response);
    }

    private static ModelInferRequest CreateModelInferRequest(TritonInferenceRequest inferRequest)
    {
        var modelInferRequest = new ModelInferRequest
                                {
                                    ModelName = inferRequest.Model,
                                    ModelVersion = inferRequest.ModelVersion
                                };

        if (!string.IsNullOrEmpty(inferRequest.RequestId))
            modelInferRequest.Id = inferRequest.RequestId;

        foreach (var tensor in inferRequest.Inputs)
        {
            var (inputTensor, rawData) = GetInputTensor(tensor);
            modelInferRequest.Inputs.Add(inputTensor);
            modelInferRequest.RawInputContents.Add(rawData);
        }

        modelInferRequest.Outputs.Add(
            inferRequest.Outputs.Select(
                o => new ModelInferRequest.Types.InferRequestedOutputTensor { Name = o }));

        SetWellKnowParameters(modelInferRequest, inferRequest);
        return modelInferRequest;
    }

    private static void SetWellKnowParameters(ModelInferRequest modelRequest, TritonInferenceRequest inferRequest)
    {
        if (inferRequest.SequenceId > 0)
        {
            modelRequest.Parameters.Add(
                TritonInferenceParameters.SequenceId,
                new InferParameter
                {
                    Int64Param = inferRequest.SequenceId
                });

            modelRequest.Parameters.Add(
                TritonInferenceParameters.SequenceStart,
                new InferParameter
                {
                    BoolParam = inferRequest.SequenceStart
                });

            modelRequest.Parameters.Add(
                TritonInferenceParameters.SequenceEnd,
                new InferParameter
                {
                    BoolParam = inferRequest.SequenceEnd
                });
        }

        if (inferRequest.Priority > 0)
        {
            modelRequest.Parameters.Add(
                TritonInferenceParameters.Priority,
                new InferParameter
                {
                    Uint64Param = inferRequest.Priority
                });
        }

        if (inferRequest.TimeoutUs > 0)
        {
            modelRequest.Parameters.Add(
                TritonInferenceParameters.Timeout,
                new InferParameter
                {
                    Int64Param = inferRequest.TimeoutUs
                });
        }
    }

    private static (ModelInferRequest.Types.InferInputTensor inputTensor, ByteString rawData) GetInputTensor(ITritonTensor tensor)
    {
        var inputTensor = new ModelInferRequest.Types.InferInputTensor
                          {
                              Name = tensor.Name,
                              Shape = { tensor.Shape },
                              Datatype = tensor.DataTypeName
                          };

        var rawData = ByteString.CopyFrom(tensor.GetRawData());
        return (inputTensor, rawData);
    }
}