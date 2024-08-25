using Unity.Entities;

public struct WaveComponent : IComponentData
{
    public float amplitude; // 振幅
    public float xOffset;
    public float yOffset;
    public float moveSpeed;
}