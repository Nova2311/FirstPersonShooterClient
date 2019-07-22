using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
public class LoginMenuManager : MonoBehaviour
{
    public static LoginMenuManager instance;

    [Header("LoginScreen")]
    [SerializeField]
    public GameObject LoginScreen;
    [SerializeField]
    TMP_InputField L_EmailInput;
    [SerializeField]
    TMP_InputField L_passwordInput;

    [Header("CreateAccountScreen")]
    public GameObject CreateAccountScreen;
    [SerializeField]
    TMP_InputField CA_UsernameInput;
    [SerializeField]
    TMP_InputField CA_EmailInput;
    [SerializeField]
    TMP_InputField CA_PasswordInput;
    [SerializeField]
    TMP_InputField CA_ConfirmPasswordInput;

    [Header("General")]
    public TMP_Text ErrorText;
    public GameObject Lobby;

    public void Awake() {
        instance = this;
    }

    public void LoginButton() {
        ErrorText.gameObject.SetActive(false);
        ClientTCP.instance.Connect("192.168.0.115", 5557);

        //ENCRYPT THE PASSWORD HERE

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteLong((long)ClientPackets.C_LoginCredentials);

        buffer.WriteString(L_EmailInput.text);
        buffer.WriteString(L_passwordInput.text);

        ClientTCP.SendData(buffer.ToArray());
    }

    public void ConfirmAccountCreationButton() {
        ErrorText.gameObject.SetActive(false);
        ClientTCP.instance.Connect("192.168.0.115", 5557);

        if (CA_PasswordInput != CA_ConfirmPasswordInput) {
            ErrorText.text = "Password's do not match";
            ErrorText.gameObject.SetActive(true);
        }

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteLong((long)ClientPackets.C_CreateAccountDetails);
        buffer.WriteString(CA_UsernameInput.text);
        buffer.WriteString(CA_EmailInput.text);
        buffer.WriteString(CA_PasswordInput.text);

        ClientTCP.SendData(buffer.ToArray());
        //send new account data
    }

    public void CreateAccountButton() {
        LoginScreen.SetActive(false);
        CreateAccountScreen.SetActive(true);
    }

    public void BackToLoginButton() {
        LoginScreen.SetActive(true);
        CreateAccountScreen.SetActive(false);
    }

    public void ExitButton() {
        Application.Quit();
    }
}
