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
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRO<TextureGridHeightPowerComponent> component in SystemAPI.Query<RefRO<TextureGridHeightPowerComponent>>())
        {
            _power = component.ValueRO.power;
            _moveSmooth = component.ValueRO.moveSmooth;
        }

        foreach (var (floatArray, emission) in SystemAPI.Query<RefRO<FloatArrayComponent>, RefRO<EmissionComponent>>())
        {
            EntityManager entityManager = state.EntityManager;
            NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp); // entities.Length = 16395

            int rows = floatArray.ValueRO.rows;
            int columns = floatArray.ValueRO.columns;
            NativeArray<float> values = floatArray.ValueRO.values;

            int index = 0;
            int count = rows * columns;
            for (int i = 0; i < entities.Length; i++)
            {
                if (entityManager.HasComponent<CubeCompoment>(entities[i]) &&
                    entityManager.HasComponent<BlockIDComponent>(entities[i]) &&
                    entityManager.HasComponent<EmissionComponent>(entities[i]))
                {
                    LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entities[i]);

                    // float pos_y = math.lerp(localTransform.Position.y, values[index] * _power, 0.5f);

                    float3 position = new float3(
                        localTransform.Position.x,
                        math.lerp(localTransform.Position.y, values[index] * _power, _moveSmooth * SystemAPI.Time.DeltaTime),
                        localTransform.Position.z
                    );

                    entityManager.SetComponentData<LocalTransform>(entities[i], LocalTransform.FromPosition(position));

                    float emissionValue = emission.ValueRO.emission + values[index];
                    entityManager.SetComponentData<EmissionComponent>(entities[i], new EmissionComponent { emission = emissionValue });

                    index++;
                }
            }
        }

        // foreach (var buffer in SystemAPI.Query<DynamicBuffer<TextureGridComponent>>())
        // {
        //     EntityManager entityManager = state.EntityManager;
        //     NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp); // entities.Length = 16395
        //     int index = 0;
        //     for (int i = 0; i < entities.Length; i++)
        //     {
        //         if (entityManager.HasComponent<CubeCompoment>(entities[i]) && entityManager.HasComponent<BlockIDComponent>(entities[i]))
        //         {
        //             LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entities[i]);

        //             float3 position = new float3(
        //                 localTransform.Position.x,
        //                 buffer[index].value * _power,
        //                 localTransform.Position.z
        //             );

        //             entityManager.SetComponentData<LocalTransform>(entities[i], LocalTransform.FromPosition(position));

        //             index++;
        //         }
        //     }
        // }
    }
}
