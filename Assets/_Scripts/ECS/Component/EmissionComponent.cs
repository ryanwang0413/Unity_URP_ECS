using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[MaterialProperty("_Emission")]
public struct EmissionComponent : IComponentData
{
    public float emission;
}