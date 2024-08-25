using UnityEngine;
using Unity.Entities;

public class TextureGridAuthoring : MonoBehaviour
{
    public float power;
    public Texture2D grayscaleImage;

    public class TextureGridBaker : Baker<TextureGridAuthoring>
    {
        public override void Bake(TextureGridAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            var buffer = AddBuffer<TextureGridComponent>(entity);
            float[,] grid = ConvertToArray(authoring.grayscaleImage);

            // 将 128x128 二维数组转换为一维数组
            for (int y = 0; y < authoring.grayscaleImage.height; y++)
            {
                for (int x = 0; x < authoring.grayscaleImage.width; x++)
                {
                    buffer.Add(new TextureGridComponent { value = grid[x, y] });
                }
            }

            // Debug.Log("[buffer count] : " + buffer.Length);

            AddComponent(entity, new TextureGridHeightPowerComponent
            {
                power = authoring.power
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
}
