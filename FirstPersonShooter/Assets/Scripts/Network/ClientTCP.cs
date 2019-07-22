using System;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;
using System.Text;

public class ClientTCP : MonoBehaviour {
    public static ClientTCP instance;

    public TcpClient client;
    public static NetworkStream myStream;
    private byte[] asyncBuffer;
    public bool isConnected;

    public byte[] receivedBytes;
    public bool handleData = false;

    public int myConnectionID;

    private void Awake() {
        UnityThread.initUnityThread();
        instance = this;
    }

    private void Start() {
    }

    public void Connect(string IP, int PORT) {
        Debug.Log("Trying to Connect to the server.");
        client = new TcpClient {
            ReceiveBufferSize = 4096,
            SendBufferSize = 4096
        };
        asyncBuffer = new byte[8192];
        try {
            client.BeginConnect(IP, PORT, new AsyncCallback(ConnectCallback), client);
        } catch {
            Debug.Log("Unable to connect to server.");
        }
    }

    public void DisconnectFromCurrentServer() {
        Debug.Log("Disconnecting from current Server!");
        client.GetStream().Close();
        myStream.Dispose();
        if (!client.Connected) {
            Debug.Log("Succesfully disconnected");
        }
    }

    private void ConnectCallback(IAsyncResult result) {
        try {
            client.EndConnect(result);
            if (client.Connected == false) {
                return;
            } else {
                isConnected = true;
                Debug.Log("You are connected to the server sucessfully.");

                myStream = client.GetStream();
                myStream.BeginRead(asyncBuffer, 0, 8192, OnReceiveData, null);

            }
        } catch (Exception e) {
            Debug.LogError(e.ToString());
            isConnected = false;
            return;
        }
    }

    private void OnReceiveData(IAsyncResult result) {
        try {
            int packetLength = myStream.EndRead(result);
            receivedBytes = new byte[packetLength];
            Buffer.BlockCopy(asyncBuffer, 0, receivedBytes, 0, packetLength);

            if (packetLength == 0) {
                isConnected = false;
                Debug.Log("Disconnected.");
                UnityThread.executeInUpdate(() => {
                    SceneManager.LoadScene("LoginScreen");
                });
                return;
            }
            UnityThread.executeInUpdate(() => {
                ClientHandleData.HandleData(receivedBytes);
            });
            myStream.BeginRead(asyncBuffer, 0, 8192, OnReceiveData, null);

        } catch (Exception) {
            //Debug.Log("Disconnected.");
            //UnityThread.executeInUpdate(() => {
            //    Application.Quit();
            //});
            return;
        }
    }

    public static void SendData(byte[] data) {
        try {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
        } catch (Exception) {
        }

    }


    #region sendData


    #endregion
}