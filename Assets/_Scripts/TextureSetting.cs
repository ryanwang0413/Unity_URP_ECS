using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class TextureSetting
{
    private static TextureSetting _instance;

    public static TextureSetting Instance
    {
        get
        {
            return _instance;
        }
    }

    public Texture2D texture;
}
