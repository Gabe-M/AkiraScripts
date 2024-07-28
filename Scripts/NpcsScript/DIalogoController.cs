using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DIalogoController : MonoBehaviour {
    [Header("Components")]
    public GameObject dialogoObj;
    public Image profile;
    public Text speechText;
    public Text actorNameText;
    public int SpeechCount;
    public int DialogoCount;
    public bool DialogoEnd;

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;



    public void SpeachNpc(Sprite p, string[] txt, string actorName) {
        dialogoObj.SetActive(true);
        profile.sprite = p;
        sentences = txt;
        actorNameText.text = actorName;
        StartCoroutine(TypeSentence());
    }
    IEnumerator TypeSentence() {
        foreach (char letter in sentences[index].ToCharArray()) {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence() {
        if (speechText.text == sentences[index]) {
            //aind ha textos
            if (index < sentences.Length - 1) {;
                index++;
                speechText.text = "";
                StartCoroutine(TypeSentence());
            }
            else {
                speechText.text = "";
                SpeechCount = 0;
                DialogoCount++;
                index = 0;
                dialogoObj.SetActive(false);
                
            }
        }
    }
}
