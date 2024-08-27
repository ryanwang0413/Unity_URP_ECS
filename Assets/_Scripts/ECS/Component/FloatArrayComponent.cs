using Unity.Collections;
using Unity.Entities;

public struct FloatArrayComponent : IComponentData
{
    public int rows;
    public int columns;
    public NativeArray<float> values;
}