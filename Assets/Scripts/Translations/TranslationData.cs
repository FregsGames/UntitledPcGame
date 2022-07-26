using System;
using System.Collections.Generic;

[Serializable]
public class Fragment
{
    public string id;
    public string spanish;
    public string english;

    public string Id { get => id; }
    public string Spanish { get => spanish; }
    public string English { get => english; }

}

[Serializable]
public class TranslationData
{
    public Fragment[] data;
}

