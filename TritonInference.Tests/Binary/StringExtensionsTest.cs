using System.Text;
using FluentAssertions;
using TritonInference.Binary;
using Xunit;

namespace TritonInference.Tests.Binary;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("ユーザー別サイト")]
    [InlineData("简体中文")]
    [InlineData("크로스 플랫폼으로")]
    [InlineData("מדורים מבוקשים")]
    [InlineData("أفضل البحوث")]
    [InlineData("Σὲ γνωρίζω ἀπὸ")]
    [InlineData("Десятую Международную")]
    [InlineData("français langue étrangère")]
    [InlineData("mañana olé")]
    [InlineData("test test")]
    public void GetUtf8ByteCount_ShouldBeCorrectlyBytesCount(string text)
    {
        text.GetUtf8ByteCount().Should().Be(Encoding.UTF8.GetByteCount(text));
    }
}