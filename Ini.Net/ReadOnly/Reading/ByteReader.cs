using System.Diagnostics.CodeAnalysis;

namespace Ini.ReadOnly;

public abstract class ByteReader
{
    [AllowNull]
    private byte[] _bytes;
    private int _pos;

    protected byte Current => _bytes[_pos];
    protected bool EndOfFile => _pos >= _bytes.Length;
    protected bool EndOfLine => Current == '\n';
    
    protected void FillBuffer(Stream stream)
    {
        _bytes = new byte[stream.Length];

        stream.Read(_bytes, 0, (int)stream.Length);
        stream.Close();

        _pos = 0;
    }

    protected void ClearBuffer()
    {
        _bytes  = Array.Empty<byte>();
        _pos    = 0;
    }

    protected void Move(int step) => _pos += step;

    protected void SkipWhitespace()
    {
        while (!EndOfFile && IsWhitespace(Current))
            Move(1);
    }

    protected bool IsWhitespace(byte c) => c == ' '
                                        || c == '\t'
                                        || c == '\n'
                                        || c == '\r'
                                        || c == '\b';

    protected void SkipLine()
    {
        while (!EndOfLine)
            Move(1);

        Move(1);
    }

    protected void SkipNewLineChar() => Move(1);
}