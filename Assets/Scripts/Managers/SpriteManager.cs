using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    [SerializeField]
    private SpritesDB spritesDB;

    public SpritesDB SpritesDB { get => spritesDB; }
}
