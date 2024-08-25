using UnityEngine;
using Unity.Entities;

public class WaveAuthoring : MonoBehaviour
{
    public float amplitude; // 振幅
    public float xOffset;
    public float yOffset;
    public float moveSpeed;
}

public class WaveBaker : Baker<WaveAuthoring>
{
    public override void Bake(WaveAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent(entity, new WaveComponent
        {
            amplitude = authoring.amplitude,
            xOffset = authoring.xOffset,
            yOffset = authoring.yOffset,
            moveSpeed = authoring.moveSpeed
        });
    }
}