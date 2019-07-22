using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;

public class ClientUDP : MonoBehaviour {
    public static ClientUDP instance;

    public static UdpClient client = new UdpClient();
    private static string IP_ADDRESS = "Localhost";
    private static int PORT = 5556;
    private static byte[] SendBuffer = new byte[4096];

    private Thread thread;

    private void Awake() {
        UnityThread.initUnityThread();
        instance = this;
    }

    public void ConnectUDP() {
        client.Connect(IP_ADDRESS, PORT);
        UnityThread.executeInUpdate(() => {
            client.BeginReceive(OnRecieve, null);
        });

    }

    static void OnRecieve(IAsyncResult result) {
        try {
            Debug.Log("Recieved UDP Data");
            IPEndPoint endpoint = null;
            byte[] data = client.EndReceive(result, ref endpoint);
            UnityThread.executeInUpdate(() => {
                HandleUDPData(data, endpoint);
            });
        } catch (Exception e) {
            Debug.LogError("Disconnected UDP With Error: " + e.ToString());
            UnityThread.executeInUpdate(() => {
                Application.Quit();
            });
            throw;
        }
        client.BeginReceive(OnRecieve, null);
    }


    static void HandleUDPData(byte[] data, IPEndPoint endpoint) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        //string splitter;
        long packet = buffer.ReadLong();
        int connectionID = buffer.ReadInteger();

        ClientHandleData.HandleDataPackets(data);
    }

    public static void StartSend(byte[] data) {
        client.BeginSend(data, data.Length, SendData, null);
    }

    public static void SendData(IAsyncResult result) {
        client.Send(SendBuffer, SendBuffer.Length, IP_ADDRESS, PORT);
    }
}
