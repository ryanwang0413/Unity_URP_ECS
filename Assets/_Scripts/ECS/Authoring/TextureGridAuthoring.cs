using UnityEngine;
using Unity.Entities;

public class TextureGridAuthoring : MonoBehaviour
{
    public float power;
    public Texture2D texture;

    public class TextureGridBaker : Baker<TextureGridAuthoring>
    {
        public override void Bake(TextureGridAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            var buffer = AddBuffer<TextureGridComponent>(entity);
            // float[,] grid = ConvertToArray(authoring.grayscaleImage);
            Texture2D target_image = authoring.texture;
            float[,] grid = ConvertToArray(target_image);

            // 将 128x128 二维数组转换为一维数组
            for (int y = 0; y < target_image.height; y++)
            {
                for (int x = 0; x < target_image.width; x++)
                {
                    buffer.Add(new TextureGridComponent { value = grid[x, y] });
                }
            }

            AddComponent(entity, new TextureGridHeightPowerComponent
            {
                power = authoring.power
            });

            // 创建一个用于存储 Texture2D 的实体
            var textureEntity = CreateAdditionalEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(textureEntity, authoring.texture); // 使用 AddComponentObject 关联 Texture2D

            // 将 Texture2D 的实体引用存储到主要实体的组件中
            AddComponent(GetEntity(authoring, TransformUsageFlags.Dynamic), new TextureUpdateComponent
            {
                TextureEntity = textureEntity
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

public struct TextureUpdateComponent : IComponentData
{
    public Entity TextureEntity;
}
