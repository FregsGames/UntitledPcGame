using System.Collections.Generic;

public interface ISaveableState
{
    Dictionary<string, string> Serialize();
    void Deserialize();
    public string ID { get;}
}
