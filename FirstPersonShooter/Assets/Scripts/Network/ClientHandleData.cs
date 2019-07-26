using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class ClientHandleData : MonoBehaviour {

    public static Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    public GameObject playerPref;
    public static GameObject playerPref_;

    public static ByteBuffer playerBuffer;
    private delegate void Packet_(byte[] data);
    private static Dictionary<long, Packet_> packets;
    private static long pLength;

    private void Awake() {
        InitializePackets();
        playerPref_ = playerPref;
    }

    private void InitializePackets() {
        Debug.Log("Initializing Network Messages...");
        packets = new Dictionary<long, Packet_>
        {
            { (long)DatabaseServerPackets.DB_EmailExistsError, EmailExistsError },
            { (long)DatabaseServerPackets.DB_UsernameExistsError, UsernameExistsError },
            { (long)DatabaseServerPackets.DB_IncorrectLoginDetails, IncorrectLoginDetails },
            { (long)DatabaseServerPackets.DB_ConfirmLoginDetails, ConfirmLoginDetails },

        };
    }

    public static void HandleData(byte[] data) {
        byte[] Buffer;
        Buffer = (byte[])data.Clone();

        if (playerBuffer == null) { playerBuffer = new ByteBuffer(); };

        playerBuffer.WriteBytes(Buffer);

        if (playerBuffer.Count() == 0) {
            playerBuffer.Clear();
            return;
        }

        if (playerBuffer.Length() >= 8) {
            pLength = playerBuffer.ReadLong(false);
            if (pLength <= 0) {
                playerBuffer.Clear();
                return;
            }
        }

        while (pLength > 0 & pLength <= playerBuffer.Length() - 8) {
            if (pLength <= playerBuffer.Length() - 8) {
                playerBuffer.ReadLong(); //Reads out the Packet Identifier;
                data = playerBuffer.ReadBytes((int)pLength); // Gets the full package Length
                HandleDataPackets(data);
            }

            pLength = 0;

            if (playerBuffer.Length() >= 8) {
                pLength = playerBuffer.ReadLong(false);

                if (pLength <= 0) {
                    playerBuffer.Clear();
                    return;
                }
            }
        }
    }

    public static void HandleDataPackets(byte[] data) {
        long packetIdentifier; ByteBuffer buffer;
        Packet_ packet;

        buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        packetIdentifier = buffer.ReadLong();
        buffer.Dispose();

        if (packets.TryGetValue(packetIdentifier, out packet)) {
            packet.Invoke(data);
        }
    }


    #region RecievePackets
    void UsernameExistsError(byte[] data) {
        LoginMenuManager.instance.ErrorText.text = "That Username already exists. Please try another";
        LoginMenuManager.instance.ErrorText.gameObject.SetActive(true);
    }
    void EmailExistsError(byte[] data) {
        LoginMenuManager.instance.ErrorText.text = "That Email already exists.";
        LoginMenuManager.instance.ErrorText.gameObject.SetActive(true);
    }
    void IncorrectLoginDetails(byte[] data) {
        LoginMenuManager.instance.ErrorText.text = "Incorrect Username or Password. Please try again";
        LoginMenuManager.instance.ErrorText.gameObject.SetActive(true);
    }

    void ConfirmLoginDetails(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        long packet = buffer.ReadLong();
        int AccountID = buffer.ReadInteger();
        General.instance.AccountID = AccountID;

        //load the lobby
        LoginMenuManager.instance.Lobby.SetActive(true);
        LoginMenuManager.instance.LoginScreen.SetActive(false);
        Destroy(LoginMenuManager.instance);
    }

    #endregion
}
