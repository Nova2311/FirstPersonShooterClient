using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LobbyMenuManager : MonoBehaviour
{
    public static LobbyMenuManager instance;

    [Header("Panels")]
    public GameObject DeleteCharacterPanel;
    public GameObject CreateCharacterPanel;
    public GameObject CharacterPanels;

    [Header("Delete Character")]
    [SerializeField]
    TMP_InputField deletionInput;

    private void Start() {
        instance = this;
    }

    public void JoinGameButton() {
        //connect to matchmaking server
        //show the queueing timer UI

    }

    public void DeleteButton() {
        DeleteCharacterPanel.SetActive(true);
    }

    public void CancelDeletion() {
        deletionInput.text = "";
        DeleteCharacterPanel.SetActive(false);
    }

    public void ConfirmDeleteCharacter() {
        if (deletionInput.text == "Delete" ) {
            //connect to database
            //tell the server to delete the character
            //delete the character from the selection list
            Debug.Log("Deleting Character");
        }
    }

    public void CreateCharacterButton() {
        DeleteCharacterPanel.SetActive(false);
        CharacterPanels.SetActive(false);
        CreateCharacterPanel.SetActive(true);
    }

    public void BackToCharacterSelect() {
        CharacterPanels.SetActive(true);
        CreateCharacterPanel.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ConfirmCreateCharacter() {
        //connect to database
        //send the character info
    }
}
