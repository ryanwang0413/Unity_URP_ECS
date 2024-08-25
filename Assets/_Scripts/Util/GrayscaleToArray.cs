using UnityEngine;

public class GrayscaleToArray : MonoBehaviour
{
    public Texture2D grayscaleImage;
    public float[,] grayscaleArray;

    void Start()
    {
        //grayscaleArray = ConvertToArray(grayscaleImage);
        Debug.Log(ConvertToArray(grayscaleImage).Length);
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