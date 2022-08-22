using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprites DB", menuName = "ScriptableObjects/Sprites DB", order = 2)]
public class SpritesDB : SerializedScriptableObject
{
    [SerializeField]
    private Dictionary<string, Sprite> sprites;

    [SerializeField]
    private Dictionary<string, Texture2D> textures;

    public Sprite GetSprite(string id)
    {
        if (sprites.ContainsKey(id))
        {
            return sprites[id];
        }
        else
        {
            return sprites.FirstOrDefault(s => s.Key != null).Value;
        }
    }

    public Texture2D GetTexture(string id)
    {
        if (textures.ContainsKey(id))
        {
            return textures[id];
        }
        else
        {
            return textures.FirstOrDefault(s => s.Key != null).Value;
        }
    }

    public string GetID(Sprite sprite)
    {
        string key = sprites.FirstOrDefault(s => s.Value == sprite).Key;
        return key;
    }
}
