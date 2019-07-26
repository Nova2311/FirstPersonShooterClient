using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using TMPro;

public class CharacterCreation : MonoBehaviour
{
    public static CharacterCreation instance;

    [Header("Panels")]
    public GameObject GenderPanel;
    public GameObject BodyPanel;
    public GameObject FacePanel;
    public GameObject HairPanel;

    [Header("Body Sliders")]
    public GameObject heightObject;
    public GameObject armSizeObject;
    public GameObject shoulderSizeObject;
    public GameObject chestSizeObject;
    public GameObject stomachSizeObject;
    public GameObject waistSizeObject;
    public GameObject legSizeObject;
    [Header("Face Sliders")]
    [Header("Hair Sliders?")]
    [Header("Avatar - DO NOT CHANGE!")]
    public DynamicCharacterAvatar avatar;
    public TMP_InputField characterNameInput;

    private Dictionary<string, DnaSetter> dna;


    // Start is called before the first frame update
    void Start(){
        instance = this;
    }


    #region change panels
    public void ChangeToGenderPanel() {
        GenderPanel.SetActive(true);
        BodyPanel.SetActive(false);
        FacePanel.SetActive(false);
        HairPanel.SetActive(false);
    }

    public void ChangeToBodyPanel() {
        GenderPanel.SetActive(false);
        BodyPanel.SetActive(true);
        FacePanel.SetActive(false);
        HairPanel.SetActive(false);
    }

    public void ChangeToFacePanel() {
        GenderPanel.SetActive(false);
        BodyPanel.SetActive(false);
        FacePanel.SetActive(true);
        HairPanel.SetActive(false);
    }

    public void ChangeToHairPanel() {
        GenderPanel.SetActive(false);
        BodyPanel.SetActive(false);
        FacePanel.SetActive(false);
        HairPanel.SetActive(true);
    }


    #endregion


    public void CreateCharacterButton() {
        //connect to database
        //send the character info
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteLong((long)ClientPackets.C_CreateCharacterDetails);
        //check the length of the name etc
        buffer.WriteInteger(General.instance.AccountID);
        buffer.WriteString(characterNameInput.text);
        buffer.WriteFloat(dna["height"].Get());
        buffer.WriteFloat(dna["armWidth"].Get());
        buffer.WriteFloat(dna["upperMuscle"].Get());
        buffer.WriteFloat(dna["upperWeight"].Get());
        buffer.WriteFloat(dna["belly"].Get());
        buffer.WriteFloat(dna["waist"].Get());
        buffer.WriteFloat(dna["lowerMuscle"].Get());

        ClientTCP.SendData(buffer.ToArray());

        Debug.Log("Create Character");
    }

    private void OnEnable() {
        avatar.CharacterUpdated.AddListener(Updated);
        #region Body
        heightObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(HeightChanged);
        armSizeObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(ArmSizeChanged);
        shoulderSizeObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(ShoulderSizeChanged);
        chestSizeObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(ChestSizeChanged);
        stomachSizeObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(StomachSizeChanged);
        waistSizeObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(WaistSizeChanged);
        legSizeObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(LegsSizeChanged);
        #endregion
    }

    private void OnDisable() {
        avatar.CharacterUpdated.RemoveListener(Updated);
        #region Body
        heightObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(HeightChanged);
        armSizeObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(ArmSizeChanged);
        shoulderSizeObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(ShoulderSizeChanged);
        chestSizeObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(ChestSizeChanged);
        stomachSizeObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(StomachSizeChanged);
        waistSizeObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(WaistSizeChanged);
        legSizeObject.GetComponentInChildren<Slider>().onValueChanged.RemoveListener(LegsSizeChanged);
        #endregion
    }

    public void SwitchGender(bool ismale) {
        if (ismale && avatar.activeRace.name != "HumanMaleDCS")
            avatar.ChangeRace("HumanMaleDCS");

        if (!ismale && avatar.activeRace.name != "HumanFemaleDCS")
            avatar.ChangeRace("HumanFemaleDCS");       
    }

    void Updated(UMAData data) {
        dna = avatar.GetDNA();
        heightObject.GetComponentInChildren<Slider>().value = dna["height"].Get();
        armSizeObject.GetComponentInChildren<Slider>().value = dna["armWidth"].Get();
        shoulderSizeObject.GetComponentInChildren<Slider>().value = dna["upperMuscle"].Get();
        chestSizeObject.GetComponentInChildren<Slider>().value = dna["upperWeight"].Get();
        stomachSizeObject.GetComponentInChildren<Slider>().value = dna["belly"].Get();
        waistSizeObject.GetComponentInChildren<Slider>().value = dna["waist"].Get();
        legSizeObject.GetComponentInChildren<Slider>().value = dna["lowerMuscle"].Get();
    }

    #region Body Values
    public void HeightChanged(float val) {
        dna["height"].Set(val);
        avatar.BuildCharacter();
    }
    public void ArmSizeChanged(float val) {
        dna["armWidth"].Set(val);
        dna["forearmWidth"].Set(val);
        avatar.BuildCharacter();
    }
    public void ShoulderSizeChanged(float val) {
        dna["upperMuscle"].Set(val);
        avatar.BuildCharacter();
    }
    public void ChestSizeChanged(float val) {
        dna["upperWeight"].Set(val);
        avatar.BuildCharacter();
    }
    public void StomachSizeChanged(float val) {
        dna["belly"].Set(val);
        avatar.BuildCharacter();
    }
    public void WaistSizeChanged(float val) {
        dna["waist"].Set(val);
        avatar.BuildCharacter();
    }
    public void LegsSizeChanged(float val) {
        dna["lowerMuscle"].Set(val);
        dna["lowerWeight"].Set(val);
        avatar.BuildCharacter();
    }
    #endregion
}
