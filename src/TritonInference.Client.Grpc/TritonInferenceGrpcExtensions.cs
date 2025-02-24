using Inference;

namespace TritonInference.Client.Grpc;

public static class TritonInferenceGrpcExtensions
{
    public static ModelMetadata MapToModelMetadata(this ModelMetadataResponse response)
    {
        return new ModelMetadata(
            Name: response.Name,
            Versions: response.Versions,
            Platform: response.Platform,
            Inputs: response.Inputs.Select(MapToTensorMetadata).ToArray(),
            Outputs: response.Outputs.Select(MapToTensorMetadata).ToArray());
    }

    private static TensorMetadata MapToTensorMetadata(ModelMetadataResponse.Types.TensorMetadata meta)
        => new(meta.Name, meta.Datatype, meta.Shape);
}