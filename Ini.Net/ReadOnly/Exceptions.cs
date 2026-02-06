namespace Ini.ReadOnly;

[Serializable]
public class CantFindSectionException : Exception
{
    public CantFindSectionException(string section) : base(section) 
    {
        Data.Add("Section", section);
    }

    protected CantFindSectionException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable]
public class CantFindValueException : Exception
{
    public CantFindValueException(string value) : base(value) 
    {
        Data.Add("Value", value);
    }

    protected CantFindValueException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}