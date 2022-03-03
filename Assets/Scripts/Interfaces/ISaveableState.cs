using System.Collections.Generic;

public interface ISaveableState
{
    void Serialize();
    void Deserialize();
    public string ID { get;}
}
