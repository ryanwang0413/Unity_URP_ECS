using System;
using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_BlockID")]
public struct BlockIDComponent : IComponentData
{
    public float blockID;
}
