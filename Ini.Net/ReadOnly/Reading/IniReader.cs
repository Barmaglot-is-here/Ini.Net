namespace Ini.ReadOnly;

public class IniReader : ByteReader
{
    private const int DEFAULT_CAPACITY = 16;

    private char[] _buffer;
    private int _bufferPos;

    private bool _allowWhitespace;

    public IniReader()
    {
        _buffer = new char[DEFAULT_CAPACITY];
    }

    public ReadOnlyIniFile Read(string file) => Read(File.OpenRead(file));

    public ReadOnlyIniFile Read(Stream stream)
    {
        FillBuffer(stream);

        var result = ReadFile();

        ClearBuffer();

        return result;
    }

    private ReadOnlyIniFile ReadFile()
    {
        Dictionary<string, ReadOnlyIniSection> sections = new();

        SkipWhitespace();

        while (!EndOfFile)
        {
            var section = ReadSection(out string name);

            sections[name] = section;
        }

        return new(sections);
    }

    private ReadOnlyIniSection ReadSection(out string name)
    {
        Dictionary<string, string> values = new();
        
        name = ReadHeader();

        SkipWhitespace();

        while (!EndOfFile && Current != Pointer.HEADER_BEGIN)
        {
            ReadLine(out var key, out var value);
            
            SkipWhitespace();
            
            if (!EndOfFile && IsCommentPointer(Current))
                SkipComments();

            values[key] = value;
        }
        
        return new(name, values);
    }

    private string ReadHeader()
    {
        Move(1); //Skip Pointer.HEADER_BEGIN
        
        while (Current != Pointer.HEADER_END)
            AddNext();

        Move(1); //Skip Pointer.HEADER_END

        return GetAndClear();
    }

    private void ReadLine(out string key, out string value)
    {
        key     = ReadKey();
        value   = ReadValue();
    }

    private string ReadKey()
    {
        while (!EndOfFile && Current != Pointer.VALUE)
            AddNext();

        Move(1); //Skip Pointer.VALUE

        return GetAndClear();
    }

    private string ReadValue()
    {
        while (!EndOfFile && !EndOfLine)
            AddNext();

        SkipNewLineChar();

        return GetAndClear();
    }

    private void AddNext()
    {
        if (IsCommentPointer(Current))
        {
            SkipComments();
            
            return;
        }

        if (Current == Pointer.CONTENT)
        {
            ToggleAllowWhitespace();

            return;
        }

        if (!IsWhitespace(Current) || _allowWhitespace)
            AddInBuffer((char)Current);

        Move(1);
    }

    private void AddInBuffer(char c)
    {
        _buffer[_bufferPos] = c;

        _bufferPos++;

        if (_bufferPos >= _buffer.Length)
            Grow();
    }

    private void Grow()
    {
        var newBuffer = new char[_buffer.Length * 2];

        _buffer.CopyTo(newBuffer, 0);

        _buffer = newBuffer;
    }

    private void ResetBuffer()
    {
        _buffer = new char[DEFAULT_CAPACITY];
        _bufferPos = 0;
    }

    private void SkipComments()
    {
        while (!EndOfFile && IsCommentPointer(Current))
        {
            SkipLine();
            SkipWhitespace();
        }
    }

    private void ToggleAllowWhitespace()
    {
        _allowWhitespace = !_allowWhitespace;

        Move(1);
    }

    private bool IsCommentPointer(byte c) => c == Pointer.COMMENT_WINDOWS || 
                                             c == Pointer.COMMENT_UNIX;

    private string GetAndClear()
    {
        string value = new(_buffer);

        ResetBuffer();

        return value;
    }
}