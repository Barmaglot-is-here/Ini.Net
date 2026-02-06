namespace Ini.ReadOnly;

public readonly struct ReadOnlyIniSection
{
    public readonly IReadOnlyDictionary<string, string> Values { get; }

    public readonly string Name { get; }

    public int ValuesCount => Values.Count;

    public string this[string key]
    {
        get
        {
            if (!Values.TryGetValue(key, out var value))
                throw new CantFindValueException(key);

            return value;
        }
    }

    internal ReadOnlyIniSection(string name, IReadOnlyDictionary<string, string> values)
    {
        Name    = name;
        Values  = values;
    }

    public bool ContainsKey(string key) => Values.ContainsKey(key);

    public bool TryGetValue(string key, out string? value) 
        => Values.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() 
        => Values.GetEnumerator();
}