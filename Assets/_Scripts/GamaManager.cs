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

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isTxStarted = false;

    [SerializeField] string IP = "127.0.0.1"; // local host
    [SerializeField] int rxPort = 8000; // port to receive data from Python on
    [SerializeField] int txPort = 8001; // port to send data to Python on

    public GameObject image;
    private Texture2D tex;
    public Texture2D tex_sample;

    public Texture2D GetTargetImage()
    {
        if (!tex)
        {
            Debug.Log("tex_sample");
            return tex_sample;
        }
        else
        {
            Debug.Log("tex");
            return tex;
        }
    }

    public Texture2D GetTargetImageTest()
    {
        return tex_sample;
    }

    // Create necessary UdpClient objects
    UdpClient client;
    IPEndPoint remoteEndPoint;
    Thread receiveThread; // Receiving Thread

    // IEnumerator SendDataCoroutine() // DELETE THIS: Added to show sending data from Unity to Python via UDP
    // {
    //     while (true)
    //     {
    //         SendData("Sent from Unity: " + i.ToString());
    //         i++;
    //         yield return new WaitForSeconds(1f);
    //     }
    // }

    public void SendData(string message) // Use to send data to Python
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    void Awake()
    {
        tex = new Texture2D(64, 64);
        // image.GetComponent<Renderer>().material.mainTexture = tex;
        image.GetComponent<MeshRenderer>().material.mainTexture = tex;

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

    bool imageReady = false;
    byte[] imageBuffer;
    private void Update()
    {
        if (imageReady)
        {
            // print(">> " + imageBuffer);
            // byte[] imageBytes = Convert.FromBase64String(rec_text);
            tex.LoadImage(imageBuffer);
            tex.Apply();

            imageReady = false;
        }
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
                // print(">> " + text);

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

}