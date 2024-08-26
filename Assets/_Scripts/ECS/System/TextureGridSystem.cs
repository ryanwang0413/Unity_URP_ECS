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
        foreach (var (textureComponent, entity) in SystemAPI.Query<RefRO<TextureUpdateComponent>>().WithEntityAccess())
        {
            var entityManager = state.EntityManager;

            Entity textureEntity = textureComponent.ValueRO.TextureEntity;
            Texture2D texture = entityManager.GetComponentObject<Texture2D>(textureEntity);
            // Example: Modify or replace the texture dynamically
            if (texture != null)
            {
                Debug.Log("got texture");
                // // 假设你有一个新的纹理 newTexture
                // Texture2D newTexture = // 获取新的 Texture2D 实例
                // if (newTexture != null)
                // {
                //     // 更新 Texture2D
                //     entityManager.SetComponentObject(textureEntity, newTexture);
                // }
            }
        }

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
