using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using System;
using System.Buffers;

[BurstCompile]
[UpdateAfter(typeof(SpawnSystem))]
public partial struct TextureGridSystem : ISystem
{
    bool isGrid;
    int index;
    float _power;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var power in SystemAPI.Query<RefRO<TextureGridHeightPowerComponent>>())
        {
            _power = power.ValueRO.power;
        }

        foreach (var buffer in SystemAPI.Query<DynamicBuffer<TextureGridComponent>>())
        {
            EntityManager entityManager = state.EntityManager;
            NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp); // entities.Length = 16395
            int index = 0;
            for (int i = 0; i < entities.Length; i++)
            {
                if (entityManager.HasComponent<CubeCompoment>(entities[i]) && entityManager.HasComponent<BlockIDComponent>(entities[i]))
                {
                    LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entities[i]);

                    float3 position = new float3(
                        localTransform.Position.x,
                        buffer[index].value * _power,
                        localTransform.Position.z
                    );

                    entityManager.SetComponentData<LocalTransform>(entities[i], LocalTransform.FromPosition(position));

                    index++;
                }
            }
        }
    }
}
