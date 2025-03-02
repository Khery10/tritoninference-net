using FluentAssertions;
using TritonInference.Client.Tensors;
using Xunit;

namespace TritonInference.Tests.Tensors;

public class StringTritonTensorTest
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void SerializeDeserialize_ShouldBeCorrectly(IReadOnlyList<string> data)
    {
        var inputTensor = new StringTritonTensor("test", [1], data);
        var outputTensor = new StringTritonTensor("test", [1], inputTensor.GetRawData().ToArray());

        outputTensor.GetData().Should().BeEquivalentTo(data);
    }

    public static TheoryData<IReadOnlyList<string>> TestData()
    {
        var theoryData = new TheoryData<IReadOnlyList<string>>();
        theoryData.Add(["sample_1", "sample_2"]);
        theoryData.Add(["français", "langue", "étrangèr"]);
        theoryData.Add(["Σὲ", "γνωρίζω", "ἀπὸ"]);
        return theoryData;
    }
}