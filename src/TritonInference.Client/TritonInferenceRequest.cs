using TritonInference.Client.Tensors;

namespace TritonInference.Client;

public record TritonInferenceRequest(
    string Model,
    string ModelVersion)
{
    private readonly List<ITritonTensor> _inputTensors = new();
    private readonly List<string> _outputTensors = new();
    public IReadOnlyList<ITritonTensor> Inputs => _inputTensors;
    public IReadOnlyList<string> Outputs => _outputTensors;
    public string? RequestId { get; init; }
    public long SequenceId { get; init; } = 0;
    public bool SequenceStart { get; init; } = false;
    public bool SequenceEnd { get; init; } = false;
    public ulong Priority { get; init; } = 0;
    public long TimeoutUs { get; init; } = 0;

    public TritonInferenceRequest AddInputTensor<TDataType>(
        string name,
        IReadOnlyList<long> shape,
        TensorDataType<TDataType> dataType,
        IReadOnlyList<TDataType> data)
        where TDataType : struct
    {
        _inputTensors.AddPrimitivesTensor(name, shape, dataType, data);
        return this;
    }

    public TritonInferenceRequest AddStringInputTensor(
        string name,
        IReadOnlyList<long> shape,
        IReadOnlyList<string> data)
    {
        _inputTensors.AddStringTensor(name, shape, data);
        return this;
    }

    public TritonInferenceRequest AddBf16InputTensor(
        string name,
        IReadOnlyList<long> shape,
        IReadOnlyList<float> data)
    {
        _inputTensors.AddBf16Tensor(name, shape, data);
        return this;
    }

    public TritonInferenceRequest AddOutputTensor(string outputTensorName)
    {
        _outputTensors.Add(outputTensorName);
        return this;
    }

    public TritonInferenceRequest AddOutputTensors(IEnumerable<string> outputTensors)
    {
        _outputTensors.AddRange(outputTensors);
        return this;
    }
}