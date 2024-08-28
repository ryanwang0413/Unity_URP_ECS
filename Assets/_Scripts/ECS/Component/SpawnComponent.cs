using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpawnComponent : IComponentData
{
    public int grid_rows;
    public int grid_columns;
    public float padding;
    public Entity singlePrefab;
    public PrefabArray prefabArray;
}
