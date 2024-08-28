using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isTxStarted = false;

    [SerializeField] string IP = "127.0.0.1"; // local host
    [SerializeField] int rxPort = 8000; // port to receive data from Python on
    [SerializeField] int txPort = 8001; // port to send data to Python on

    private EntityManager _entityManager;
    public Entity _entity;
    private Texture2D updateTexture; // 持續更新的圖片
    public Texture2D defaultTexture; // 預設使用的圖片
    public Texture2D GetTargetImage()
    {
        if (!updateTexture) { return defaultTexture; }
        else { return updateTexture; }
    }

    // 傳給 Entity 的資料
    private float[,] _floatArray;
    private NativeArray<float> _nativeArray;
    bool imageReady = false;
    byte[] imageBuffer;

    // Socket
    UdpClient client;
    IPEndPoint remoteEndPoint;
    Thread receiveThread; // Receiving Thread

    // public void SendData(string message) // Use to send data to Python
    // {
    //     try
    //     {
    //         byte[] data = Encoding.UTF8.GetBytes(message);
    //         client.Send(data, data.Length, remoteEndPoint);
    //     }
    //     catch (Exception err)
    //     {
    //         print(err.ToString());
    //     }
    // }

    void Awake()
    {
        updateTexture = new Texture2D(320, 180);

        // Create remote endpoint (to Matlab) 
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), txPort);

        // Create local client
        client = new UdpClient(rxPort);

        // local endpoint define (where messages are received)
        // Create a new thread for reception of incoming messages
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        // Initialize (seen in comments window)
        print("UDP Comms Initialised");

        //StartCoroutine(SendDataCoroutine()); // DELETE THIS: Added to show sending data from Unity to Python via UDP
    }

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // 動態建立一組 Entity 加入 Component
        _entity = _entityManager.CreateEntity(typeof(FloatArrayComponent));
        _floatArray = ConvertToArray(defaultTexture);
        Debug.Log("defaultTexture size : " + defaultTexture.width + " | " + defaultTexture.height);
        // 初始化資料
        _nativeArray = new NativeArray<float>(_floatArray.Length, Allocator.Persistent);
        UpdateComponentData(_floatArray);
    }

    private void Update()
    {
        if (imageReady)
        {
            // print(">> " + imageBuffer);
            // byte[] imageBytes = Convert.FromBase64String(rec_text);
            updateTexture.LoadImage(imageBuffer);
            updateTexture.Apply();

            imageReady = false;
        }

        _floatArray = ConvertToArray(GetTargetImage());
        UpdateComponentData(_floatArray);

    }

    private void UpdateComponentData(float[,] _imageArray)
    {
        int rows = _imageArray.GetLength(0);
        int columns = _imageArray.GetLength(1);

        // float[,] 轉為 NativeArray
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _nativeArray[i * columns + j] = _imageArray[i, j];
            }
        }

        // 更新 Entity 中的目標 Component
        _entityManager.SetComponentData(_entity, new FloatArrayComponent
        {
            rows = rows,
            columns = columns,
            values = _nativeArray
        });
    }

    // Receive data, update packets received
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                imageBuffer = data.ToArray();
                // string text = Encoding.UTF8.GetString(data);
                // print(">> " + imageBuffer.Length);
                imageReady = true;
                // rec_text = text;
                // ProcessInput(text);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private void ProcessInput(string input)
    {
        // PROCESS INPUT RECEIVED STRING HERE

        if (!isTxStarted) // First data arrived so tx started
        {
            isTxStarted = true;
        }
    }

    //Prevent crashes - close clients and threads properly!
    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
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

    private void OnDestroy()
    {
        if (_nativeArray.IsCreated)
        {
            _nativeArray.Dispose();
        }
    }
}