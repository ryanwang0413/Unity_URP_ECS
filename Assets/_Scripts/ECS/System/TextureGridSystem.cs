using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using System;
using System.Buffers;
using Unity.Rendering;

[BurstCompile]
[UpdateAfter(typeof(SpawnSystem))]
public partial struct TextureGridSystem : ISystem
{
    float _power;
    float _moveSmooth;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp); // entities.Length = 16395

        foreach (RefRO<TextureGridHeightPowerComponent> component in SystemAPI.Query<RefRO<TextureGridHeightPowerComponent>>())
        {
            _power = component.ValueRO.power;
            _moveSmooth = component.ValueRO.moveSmooth;
        }

        foreach (var floatArray in SystemAPI.Query<RefRO<FloatArrayComponent>>())
        {
            int rows = floatArray.ValueRO.rows;
            int columns = floatArray.ValueRO.columns;
            NativeArray<float> values = floatArray.ValueRO.values;

            int index = 0;
            int count = rows * columns;
            for (int i = 0; i < entities.Length; i++)
            {
                if (entityManager.HasComponent<CubeCompoment>(entities[i]) &&
                    entityManager.HasComponent<BlockIDComponent>(entities[i]))
                {
                    LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entities[i]);

                    float clampedValue = Mathf.Clamp01(values[index]);
                    float multiplier = _power * Mathf.Pow(2f, clampedValue);
                    float amplifiedValue = clampedValue * multiplier;

                    float3 position = new float3(
                        localTransform.Position.x,
                        math.lerp(localTransform.Position.y, amplifiedValue, _moveSmooth * SystemAPI.Time.DeltaTime),
                        localTransform.Position.z
                    );

                    entityManager.SetComponentData<LocalTransform>(entities[i], LocalTransform.FromPosition(position));

                    index++;
                }
            }
        }

        // 非常 lag...改用 IJobEntity
        // foreach (var emission in SystemAPI.Query<RefRW<EmissionComponent>>())
        // {
        //     int index = 0;
        //     for (int i = 0; i < entities.Length; i++)
        //     {
        //         if (entityManager.HasComponent<EmissionComponent>(entities[i]))
        //         {
        //             // float emissionValue = -2;
        //             // emission.ValueRW.emission = emissionValue;
        //             entityManager.SetComponentData<EmissionComponent>(entities[i], new EmissionComponent { emission = -2 });

        //             index++;
        //         }
        //     }
        // }
    }
}
