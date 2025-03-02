namespace TritonInference.Client.Tensors;

public static class TritonTensorExtensions
{
    public static void AddPrimitivesTensor<TDataType>(
        this IList<ITritonTensor> tensors,
        string name,
        IReadOnlyList<long> shape,
        TensorDataType<TDataType> dataType,
        IReadOnlyList<TDataType> data)
        where TDataType : struct =>
        tensors.Add(new PrimitivesTritonTensor<TDataType>(name, shape, dataType, data));

    public static void AddPrimitivesTensor<TDataType>(
        this IList<ITritonTensor> tensors,
        string name,
        IReadOnlyList<long> shape,
        TensorDataType<TDataType> dataType,
        ReadOnlyMemory<byte> rawData)
        where TDataType : struct =>
        tensors.Add(new PrimitivesTritonTensor<TDataType>(name, shape, dataType, rawData));

    public static void AddStringTensor(
        this IList<ITritonTensor> tensors,
        string name,
        IReadOnlyList<long> shape,
        IReadOnlyList<string> data) =>
        tensors.Add(new StringTritonTensor(name, shape, data));

    public static void AddStringTensor(
        this IList<ITritonTensor> tensors,
        string name,
        IReadOnlyList<long> shape,
        ReadOnlyMemory<byte> rawData) =>
        tensors.Add(new StringTritonTensor(name, shape, rawData));

    public static void AddBf16Tensor(
        this IList<ITritonTensor> tensors,
        string name,
        IReadOnlyList<long> shape,
        IReadOnlyList<float> data) =>
        tensors.Add(new Bf16TritonTensor(name, shape, data));

    public static void AddBf16Tensor(
        this IList<ITritonTensor> tensors,
        string name,
        IReadOnlyList<long> shape,
        ReadOnlyMemory<byte> rawData) =>
        tensors.Add(new Bf16TritonTensor(name, shape, rawData));
}