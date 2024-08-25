using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;

[BurstCompile]
public partial struct WaveSystem : ISystem
{
    public float _amplitude; // 振幅
    public float _xOffset;
    public float _yOffset;
    public float _moveSpeed;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);
        foreach (Entity entity in entities)
        {
            // if (entityManager.HasComponent<WaveComponent>(entity))
            // {
            //     WaveComponent waveComponent = entityManager.GetComponentData<WaveComponent>(entity);
            //     _amplitude = waveComponent.amplitude;
            //     _xOffset = waveComponent.xOffset;
            //     _yOffset = waveComponent.yOffset;
            //     _moveSpeed = waveComponent.moveSpeed;
            // }
            // else if (entityManager.HasComponent<CubeCompoment>(entity) && entityManager.HasComponent<BlockIDComponent>(entity))
            // {
            //     LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entity);

            //     float yPosition =
            //     _amplitude * math.sin(
            //         (float)SystemAPI.Time.ElapsedTime * _moveSpeed + localTransform.Position.x * _xOffset + localTransform.Position.z * _yOffset);

            //     float3 position = new float3(
            //         localTransform.Position.x,
            //         yPosition,
            //         localTransform.Position.z
            //     );

            //     entityManager.SetComponentData<LocalTransform>(entity, LocalTransform.FromPosition(position));
            // }
        }
    }
}
