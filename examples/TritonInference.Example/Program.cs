// See https://aka.ms/new-console-template for more information

using TritonInference.Client;
using TritonInference.Client.Grpc;
using TritonInference.Client.Tensors;

var client = new TritonInferenceGrpcClient("http://localhost:8001");

var isReady = await client.IsServerReady();
Console.WriteLine($"Server is ready: {isReady}");

var serverMetadata = await client.GetServerMetadata();
Console.WriteLine(serverMetadata);

var models = await client.RepositoryIndex();
foreach (var model in models)
    Console.WriteLine(model);

var request = new TritonInferenceRequest("llama_test_model", "1")
              .AddInputTensor("input_ids", [1, 4], TensorDataTypes.Int32, [1, 65536, 3906, 3])
              .AddInputTensor("request_output_len", [1, 1], TensorDataTypes.Int32, [10])
              .AddInputTensor("beam_width", [1, 1], TensorDataTypes.Int32, [8])
              .AddInputTensor("end_id", [1, 1], TensorDataTypes.Int32, [2])
              .AddInputTensor("pad_id", [1, 1], TensorDataTypes.Int32, [8])
              .AddOutputTensors(["output_ids", "sequence_length"]);

var result = await client.Infer(request, CancellationToken.None);
Console.WriteLine("OutputIds: " + string.Join(",", result.GetOutputTensor<int>("output_ids").GetData()));
Console.WriteLine("SequenceLength: " + string.Join(",", result.GetOutputTensor<int>("sequence_length").GetData()));