namespace TritonInference.Client;

public record TensorDataType<T>(string TypeName, int NumBytes, bool Signed);