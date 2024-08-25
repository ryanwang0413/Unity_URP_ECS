using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpawnComponent : IComponentData
{
    public int gridCound;
    public float padding;
    public Entity singlePrefab;
    public PrefabArray prefabArray;
}
