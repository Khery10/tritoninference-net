namespace TritonInference.Client.Tensors;

public static class TensorDataTypes
{
    public static readonly TensorDataType<bool> Bool = new("BOOL", 1);
    public static readonly TensorDataType<sbyte> Int8 = new("INT8", 1);
    public static readonly TensorDataType<short> Int16 = new("INT16", 2);
    public static readonly TensorDataType<int> Int32 = new("INT32", 4);
    public static readonly TensorDataType<long> Int64 = new("INT64", 8);
    public static readonly TensorDataType<byte> Uint8 = new("UINT8", 1);
    public static readonly TensorDataType<ushort> Uint16 = new("UINT16", 2);
    public static readonly TensorDataType<uint> Uint32 = new("UINT32", 4);
    public static readonly TensorDataType<ulong> Uint64 = new("UINT64", 8);
    public static readonly TensorDataType<float> Fp16 = new("FP16", 2);
    public static readonly TensorDataType<float> Fp32 = new("FP32", 4);
    public static readonly TensorDataType<double> Fp64 = new("FP64", 8);
    public static readonly TensorDataType<string> Bytes = new("BYTES", -1);
    public static readonly TensorDataType<float> Bf16 = new("BF16", 2);

    public static TensorDataType<TDataType> GetDataType<TDataType>(string typeName)
    {
        if (Bool.IsType(typeName, out TensorDataType<TDataType>? dataType))
            return dataType;
        if (Int8.IsType(typeName, out dataType))
            return dataType;
        if (Int16.IsType(typeName, out dataType))
            return dataType;
        if (Int32.IsType(typeName, out dataType))
            return dataType;
        if (Int64.IsType(typeName, out dataType))
            return dataType;
        if (Uint8.IsType(typeName, out dataType))
            return dataType;
        if (Uint16.IsType(typeName, out dataType))
            return dataType;
        if (Uint32.IsType(typeName, out dataType))
            return dataType;
        if (Uint64.IsType(typeName, out dataType))
            return dataType;
        if (Fp16.IsType(typeName, out dataType))
            return dataType;
        if (Fp32.IsType(typeName, out dataType))
            return dataType;
        if (Fp64.IsType(typeName, out dataType))
            return dataType;
        if (Bytes.IsType(typeName, out dataType))
            return dataType;
        if (Bf16.IsType(typeName, out dataType))
            return dataType;

        throw new InvalidCastException($"Can not get {typeName} whit type {typeof(TDataType)}");
    }
}