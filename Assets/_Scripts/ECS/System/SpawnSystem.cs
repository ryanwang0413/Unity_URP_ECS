using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.VisualScripting;
using Unity.Rendering;
using Unity.Collections;

[BurstCompile]
public partial struct SpawnSystem : ISystem
{
    bool isSpawned;
    int blockIndex;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        isSpawned = false;
        blockIndex = 0;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 查詢所有 SpawnComponent 元件。
        // 使用 RefRW 因為系統想要讀取和寫入元件。如果系統只需要唯讀訪問，它將使用 RefRO 代替。
        if (isSpawned == false)
        {
            foreach (var spawner in SystemAPI.Query<RefRW<SpawnComponent>>())
            {
                ProcessSpawner(ref state, spawner);
                isSpawned = true;
                // Debug.Log(buffer.Length);
            }
        }
    }

    [BurstCompile]
    private void ProcessSpawner(ref SystemState state, RefRW<SpawnComponent> spawner)
    {
        int bufferIndex = 0;
        for (int i = 0; i < spawner.ValueRO.grid_rows; i++)
        {
            for (int j = 0; j < spawner.ValueRO.grid_columns; j++)
            {
                //Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.prefabArray.GetPrefab(i));
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.singlePrefab);

                // 計算格子的位置
                float posX = i * spawner.ValueRO.padding - spawner.ValueRO.grid_rows / 2;
                float posZ = j * spawner.ValueRO.padding - spawner.ValueRO.grid_columns / 2;

                float3 position = new float3(
                    posX,
                    0,
                    posZ
                );

                // 新增或設置 component
                GetEntityCommandBuffer(ref state).SetComponent(0, newEntity, LocalTransform.FromPosition(position));

                GetEntityCommandBuffer(ref state).AddComponent(0, newEntity, new BlockIDComponent
                {
                    blockID = blockIndex
                });

                GetEntityCommandBuffer(ref state).AddComponent(0, newEntity, new CubeTiling
                {
                    Value = new float2(1, 1)
                });

                GetEntityCommandBuffer(ref state).AddComponent(0, newEntity, new CubeOffset
                {
                    Value = new float2(3, 3)
                });

                GetEntityCommandBuffer(ref state).AddComponent(0, newEntity, new CubeTransparency
                {
                    Value = 0
                });

                GetEntityCommandBuffer(ref state).AddComponent(0, newEntity, new EmissionComponent
                {
                    emission = 0
                });

                bufferIndex++;
            }
        }
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }


}