namespace Ini.ReadOnly;

public readonly struct ReadOnlyIniFile
{
    public readonly IReadOnlyDictionary<string, ReadOnlyIniSection> Sections { get; }

    public int SectionsCount => Sections.Count;

    public ReadOnlyIniSection this[string key]
    {
        get
        {
            if (!Sections.TryGetValue(key, out var value))
                throw new CantFindSectionException(key);

            return value;
        }
    }

    public string this[string section, string key] => this[section][key];

    public static ReadOnlyIniFile Read(string file) => Read(File.OpenRead(file));
    public static ReadOnlyIniFile Read(Stream stream)
    {
        IniReader reader = new();

        return reader.Read(stream);
    }

    internal ReadOnlyIniFile(IReadOnlyDictionary<string, ReadOnlyIniSection> sections)
    {
        Sections = sections;
    }

    public bool ContainsKey(string key) => Sections.ContainsKey(key);

    public bool ContainsKey(string section, string key) =>
        ContainsKey(section) && Sections[section].ContainsKey(key);
    
    public bool TryGetValue(string key, out ReadOnlyIniSection value) =>
        Sections.TryGetValue(key, out value);

    public bool TryGetValue(string section, string key, out string? value)
    {
        if (TryGetValue(section, out var sect))
           return sect.TryGetValue(key, out value);

        value = default;

        return false;
    }

    public IEnumerator<KeyValuePair<string, ReadOnlyIniSection>> GetEnumerator() =>
        Sections.GetEnumerator();
}