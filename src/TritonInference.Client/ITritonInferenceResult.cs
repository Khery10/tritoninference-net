using TritonInference.Client.Tensors;

namespace TritonInference.Client;

public interface ITritonInferenceResult
{
    public string Id { get; }
    public string ModelName { get; }
    public string ModelVersion { get; }
    public bool TryGetParameter<TValue>(string paramName, out TValue value);
    public ITritonTensor<string> GetStringOutputTensor(string tensorName);

    public ITritonTensor<TDataType> GetOutputTensor<TDataType>(string tensorName)
        where TDataType : struct;
}