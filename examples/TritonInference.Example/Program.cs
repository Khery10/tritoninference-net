// See https://aka.ms/new-console-template for more information

using TritonInference.Client.Grpc;

var client = new TritonInferenceGrpcClient("http://localhost:8001");

var isReady = await client.IsServerReady();
Console.WriteLine($"Server is ready: {isReady}");

var serverMetadata = await client.GetServerMetadata();
Console.WriteLine(serverMetadata);

var models = await client.RepositoryIndex();
foreach (var model in models)
    Console.WriteLine(model);