using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;


public struct CubeCompoment : IComponentData
{ }

[MaterialProperty("_Tiling")]
public struct CubeTiling : IComponentData
{
    public float2 Value;
}

[MaterialProperty("_Offset")]
public struct CubeOffset : IComponentData
{
    public float2 Value;
}

[MaterialProperty("_Transparency")]
public struct CubeTransparency : IComponentData
{
    public float Value;
}
