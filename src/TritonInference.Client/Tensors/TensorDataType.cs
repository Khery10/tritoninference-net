using System.Diagnostics.CodeAnalysis;

namespace TritonInference.Client.Tensors;

public record TensorDataType<T>(string TypeName, int NumBytes)
{
    public string TypeName { get; } = TypeName;
    public int NumBytes { get; } = NumBytes;
    public bool IsType<TDataType>(string typeName, [NotNullWhen(true)] out TensorDataType<TDataType>? dataType)
    {
        dataType = null;
        if (TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase)
            && this is TensorDataType<TDataType> tensorDataType)
        {
            dataType = tensorDataType;
            return true;
        }

        return false;
    }
}