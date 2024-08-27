using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using UnityEngine.Rendering;

public class TextureGridAuthoring : MonoBehaviour
{
    public float power;
    public float moveSmooth;
    // public Texture2D texture;

    public class TextureGridBaker : Baker<TextureGridAuthoring>
    {
        public override void Bake(TextureGridAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            // DynamicBuffer<TextureGridComponent> buffer = AddBuffer<TextureGridComponent>(entity);
            // Texture2D target_image = authoring.texture;
            // float[,] grid = ConvertToArray(target_image);

            // // 二維數據轉為一維數據
            // for (int y = 0; y < 128; y++)
            // {
            //     for (int x = 0; x < 128; x++)
            //     {
            //         buffer.Add(new TextureGridComponent { value = grid[x, y] });
            //         // buffer.Add(new TextureGridComponent { value = authoring.textureArray[x, y] });
            //     }
            // }

            AddComponent(entity, new TextureGridHeightPowerComponent
            {
                power = authoring.power,
                moveSmooth = authoring.moveSmooth
            });
        }

        public float[,] ConvertToArray(Texture2D image)
        {
            int width = image.width;
            int height = image.height;

            float[,] grayscaleArray = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    float grayValue = pixelColor.grayscale;
                    grayscaleArray[x, y] = grayValue;
                }
            }

            return grayscaleArray;
        }
    }
}

public struct TextureGridComponent : IBufferElementData
{
    public float value;
}

public struct TextureGridHeightPowerComponent : IComponentData
{
    public float power;
    public float moveSmooth;
}
