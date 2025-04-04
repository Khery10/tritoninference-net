using System;
using FluentAssertions;
using TritonInference.Client.Tensors;
using Xunit;

namespace TritonInference.Tests.Tensors;

public class PrimitiveTritonTensorTest
{
    [Fact]
    public void Create_WithBf16_ShouldThrowException()
    {
        var act = () => new PrimitivesTritonTensor<float>(
                      "test",
                      new long[] { 1 },
                      TensorDataTypes.Bf16,
                      new[] { 0.1f, 0.2f });

        act.Should().Throw<ArgumentException>();
    }
}