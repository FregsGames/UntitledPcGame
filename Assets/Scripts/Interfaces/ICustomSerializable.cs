using System.Collections.Generic;

public interface ICustomSerializable
{
    Dictionary<string, string> Serialize();
    void Deserialize();
    public string ID { get;}
}
