using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading;
public class General : MonoBehaviour
{
    public static General instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    private void OnApplicationQuit()
    {
        if (ClientTCP.instance.isConnected) {
            ClientTCP.instance.client.Close();
        }
    }

    private void Update() {

    }
}
