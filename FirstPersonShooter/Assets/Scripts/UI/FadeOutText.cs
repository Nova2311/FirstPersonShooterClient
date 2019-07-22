using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FadeOutText : MonoBehaviour
{
    //Fade time in seconds
    public float fadeOutTime;

    private float waitToStartFading;
    private float timeToFade;

    private void OnEnable() {
        timeToFade = fadeOutTime / 3;
        waitToStartFading = fadeOutTime -= timeToFade; 
    }

    private void Update() {
        waitToStartFading -= Time.deltaTime; //Allows the error message to be locked onto the screen for 2 3rds of the amount of time.
        if (waitToStartFading <= 0) {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine() {
        Debug.Log("Starting CoRoutine");
        TMP_Text text = GetComponent<TMP_Text>();
        Color originalColor = text.color;
        for (float t = 0.01f; t < timeToFade; t += Time.deltaTime) {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / timeToFade));
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
