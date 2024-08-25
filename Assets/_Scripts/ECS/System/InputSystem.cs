using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;

[UpdateAfter(typeof(SpawnSystem))]
[BurstCompile]
public partial struct InputSystem : ISystem
{
    int blockIndex;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        blockIndex = 0;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (blockIndex >= 32)
            {
                blockIndex = 32;
            }
            else
            {
                blockIndex += 1;
            }

            ChangeBlock(ref state, blockIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (blockIndex <= 0)
            {
                blockIndex = 0;
            }
            else
            {
                blockIndex -= 1;
            }

            ChangeBlock(ref state, blockIndex);
        }
    }

    [BurstCompile]
    private void ChangeBlock(ref SystemState state, int blockIndex)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);
        foreach (Entity entity in entities)
        {
            if (entityManager.HasComponent<BlockIDComponent>(entity))
            {
                entityManager.SetComponentData<BlockIDComponent>(entity, new BlockIDComponent
                {
                    blockID = blockIndex
                });
            }
        }
    }
}
