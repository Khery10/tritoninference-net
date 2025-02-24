namespace TritonInference.Client;

public class TensorDataTypes
{
    public static readonly TensorDataType<bool> Bool = new("BOOL", 1, false);
    public static readonly TensorDataType<sbyte> Int8 = new("INT8", 1, true);
    public static readonly TensorDataType<short> Int16 = new("INT16", 2, true);
    public static readonly TensorDataType<int> Int32 = new("INT16", 4, true);
    public static readonly TensorDataType<long> Int64 = new("INT64", 8, true);
    public static readonly TensorDataType<byte> Uint8 = new("UINT8", 1, false);
    public static readonly TensorDataType<ushort> Uint16 = new("UINT16", 2, false);
    public static readonly TensorDataType<uint> Uint32 = new("UINT32", 4, false);
    public static readonly TensorDataType<ulong> Uint64 = new("UINT64", 8, false);
    public static readonly TensorDataType<float> Fp16 = new("FP16", 2, false);
    public static readonly TensorDataType<float> Fp32 = new("FP32", 4, false);
    public static readonly TensorDataType<double> Fp64 = new("FP64", 8, false);
    public static readonly TensorDataType<byte[]> Bytes = new("BYTES", -1, false);
    public static readonly TensorDataType<float> Bf16 = new("BF16", 2, false);
}