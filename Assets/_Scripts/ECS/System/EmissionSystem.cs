using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 處理 Cube 材質動態變化
/// 必須使用 IJobEntity 才不會 Lag
/// </summary>
[BurstCompile]
[UpdateAfter(typeof(TextureGridSystem))]
public partial struct EmissionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    { }

    public void OnUpdate(ref SystemState state)
    {
        var cubeEmissionJob = new CubeEmissionJob
        {
            value = 0,

            // TODO 關鍵：取得其他組件 Component，true=只讀
            localTransform = state.GetComponentLookup<LocalTransform>(true)
        };

        state.Dependency = cubeEmissionJob.Schedule(state.Dependency);
    }
}

[BurstCompile]
public partial struct CubeEmissionJob : IJobEntity
{
    public float value;
    [ReadOnly] public ComponentLookup<LocalTransform> localTransform; // ComponentLookup<T> 取得目標組件

    public void Execute(ref URPMaterialPropertyBaseColor baseColor, ref EmissionComponent emission, in Entity entity)
    {
        // 檢查 Entity 是否有對應組件
        if (localTransform.HasComponent(entity))
        {
            var pos_y = localTransform[entity].Position.y;

            // float normalizedValue = Mathf.InverseLerp(0f, 1f, pos_y);

            // 計算最大值
            float clampedValue = Mathf.Clamp01(1f);
            float multiplier = 15f * Mathf.Pow(2f, clampedValue);
            float amplifiedValue = clampedValue * multiplier;

            // 將高度映射到 0-1 範圍
            float normalizedHeight = Mathf.InverseLerp(0, amplifiedValue, pos_y);
            float4 result = HeightToColor(normalizedHeight);
            baseColor.Value = result;


            float normalizedValue = Mathf.InverseLerp(0f, amplifiedValue, pos_y);
            emission.emission = normalizedValue * 3;
        }
    }

    // // Define a job that processes Translation and Rotation components
    // public void Execute(ref EmissionComponent emission, in Entity entity)
    // {
    //     // 檢查 Entity 是否有對應組件
    //     if (localTransform.HasComponent(entity))
    //     {
    //         // 取得組件的值(取法特殊)
    //         var pos_y = localTransform[entity].Position.y;

    //         float normalizedValue = Mathf.Lerp(0f, 5f, pos_y);
    //         emission.emission = normalizedValue;
    //     }

    // }

    public float4 HeightToColor(float normalizedHeight)
    {
        // 確保輸入值在 0-1 範圍內
        normalizedHeight = Mathf.Clamp01(normalizedHeight);

        // 使用正規化高度作為色相（H）
        float h = normalizedHeight;

        // 從 HSV 轉換到 RGB
        // 為了不要重複頭尾顏色，扣除尾巴顏色
        Color hsvColor = Color.HSVToRGB(h / 1.15f, 1, 1);

        // 將 Color 轉換為 Vector4（float4）
        return new float4(hsvColor.r, hsvColor.g, hsvColor.b, 1f);
    }
}
