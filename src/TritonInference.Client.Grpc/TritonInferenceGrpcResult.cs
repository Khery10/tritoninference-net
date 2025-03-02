using Inference;
using TritonInference.Client.Tensors;

namespace TritonInference.Client.Grpc;

public class TritonInferenceGrpcResult(ModelInferResponse inferResponse) : ITritonInferenceResult
{
    public string Id => inferResponse.Id;
    public string ModelName => inferResponse.ModelName;
    public string ModelVersion => inferResponse.ModelVersion;

    public bool TryGetParameter<TValue>(string paramName, out TValue value)
    {
        value = default!;
        if (!inferResponse.Parameters.TryGetValue(paramName, out var paramValue))
            return false;

        if (paramValue.BoolParam is TValue boolParam)
        {
            value = boolParam;
            return true;
        }

        if (paramValue.DoubleParam is TValue doubleParam)
        {
            value = doubleParam;
            return true;
        }

        if (paramValue.Int64Param is TValue longParam)
        {
            value = longParam;
            return true;
        }

        if (paramValue.StringParam is TValue stringParam)
        {
            value = stringParam;
            return true;
        }

        if (paramValue.Uint64Param is TValue ulongParam)
        {
            value = ulongParam;
            return true;
        }

        return false;
    }

    public ITritonTensor<string> GetStringOutputTensor(string tensorName)
    {
        var (tensor, index) = GetTensorInternal(tensorName);
        var rawContent = GetRawContent(index);

        if (!rawContent.IsEmpty)
            return new StringTritonTensor(tensor.Name, tensor.Shape, rawContent);

        var data = tensor.Contents.BytesContents.Select(s => s.ToStringUtf8()).ToArray();
        return new StringTritonTensor(tensor.Name, tensor.Shape, data);
    }

    public ITritonTensor<TDataType> GetOutputTensor<TDataType>(string tensorName)
        where TDataType : struct
    {
        var (tensor, index) = GetTensorInternal(tensorName);
        var dataType = TensorDataTypes.GetDataType<TDataType>(tensor.Datatype);
        var rawContent = GetRawContent(index);

        if (dataType.Equals(TensorDataTypes.Bf16))
            return (ITritonTensor<TDataType>) new Bf16TritonTensor(tensor.Name, tensor.Shape, rawContent);

        return new PrimitivesTritonTensor<TDataType>(tensor.Name, tensor.Shape, dataType, rawContent);
    }

    private (ModelInferResponse.Types.InferOutputTensor outputTensor, int index) GetTensorInternal(string tensorName)
    {
        for (var i = 0; i < inferResponse.Outputs.Count; i++)
        {
            var tensor = inferResponse.Outputs[i];
            if (!tensor.Name.Equals(tensorName, StringComparison.OrdinalIgnoreCase))
                continue;

            return (tensor, i);
        }

        throw new ArgumentException($"Tensor {tensorName} not found");
    }

    private ReadOnlyMemory<byte> GetRawContent(int index)
    {
        return index < inferResponse.RawOutputContents.Count
            ? inferResponse.RawOutputContents[index].Memory
            : ReadOnlyMemory<byte>.Empty;
    }
}