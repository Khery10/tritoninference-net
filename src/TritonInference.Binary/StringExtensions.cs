namespace TritonInference.Binary;

public static class StringExtensions
{
    public static int GetUtf8ByteCount(this string text)
    {
        unsafe
        {
            fixed (char* chars = text)
            {
                return GetUtf8ByteCount(chars, text.Length);
            }
        }
    }

    private static unsafe int GetUtf8ByteCount(char* chars, int strLen)
    {
        int utfLen = 0;
        int cnt;
        for (cnt = 0; cnt < strLen; cnt++)
        {
            var c = *(chars + cnt);

            // ASCII
            if (c >= 0x0001 && c <= 0x007F)
                utfLen++;
            // Special symbols (surrogates)
            else if (c > 0x07FF)
                utfLen += 3;
            // The rest of the symbols.
            else
                utfLen += 2;
        }

        return utfLen;
    }
}