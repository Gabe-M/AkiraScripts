using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogo1 : MonoBehaviour{
    public Sprite profile;
    #region speechText
    public string[] speechText1;
    public string[] speechText2;
    public string[] speechText3;
    public string[] speechText4;
    public string[] speechText6;
    public string[] speechText7;
    public string[] speechText8;
    #endregion
    [SerializeField] private int dialogoCount;
  //  public int speechCount;
    public string actorName;
    public GameObject[] Scene;

    [SerializeField] private LayerMask dialogoPoint;
    public LayerMask playerLayer;
    public float radius;
    public bool onDialogo;
    private DIalogoController dc;
    public bool startCene = false;
    private int sceneCount;
    //public bool dialogoEnd;

    bool onRadius;
    void Start(){
        dc = FindAnyObjectByType<DIalogoController>();


    }


    void Update(){
        dialogoCount = FindAnyObjectByType<DIalogoController>().DialogoCount;
        Dialogo();
        TimeLine();
    }
    private void FixedUpdate() {
        Interact();
    }
    public void Interact() {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if(hit != null) { 
            onRadius = true ;
        }

        else { onRadius = false; }

    }
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void Dialogo() {
        Collider2D dialogoPointDetecter = Physics2D.OverlapCircle(transform.position, radius, dialogoPoint);

        if (onRadius && dialogoPointDetecter) {
            if (dialogoCount != 4) {
                Destroy(dialogoPointDetecter.gameObject);
            }
            switch (dialogoCount) {
                case 0: {
                        dc.SpeachNpc(profile, speechText1, actorName);
                        break;
                    }
                case 1: {
                        dc.SpeachNpc(profile, speechText2, actorName);
                        break;
                    }
                case 2: {
                        dc.SpeachNpc(profile, speechText3, actorName);
                        break;
                    }
                case 3: {
                        dc.SpeachNpc(profile, speechText4, actorName);
                        break;
                    }
                case 5: {
                        dc.SpeachNpc(profile, speechText6, actorName);
                        break;
                    }
                case 6: {
                        dc.SpeachNpc(profile, speechText7, actorName);
                        break;
                    }
                case 7: {
                     dc.SpeachNpc(profile, speechText8, actorName);
                        break;
                    }
                    /*   case 7: {
                               dc.SpeachNpc(profile, speechText8, actorName);
                               break;
                        }*/
            }

        }
        if (Input.GetKeyDown(KeyCode.Return) && onRadius) {
            dc.NextSentence();

        }


    }

    void TimeLine() {
        if (dialogoCount == 3 && !Scene[0].active && sceneCount == 0) {
            Scene[0].SetActive(true);
            sceneCount++;
        }
        if (dialogoCount == 4 && !Scene[1].active && sceneCount == 1) {
            Scene[1].SetActive(true);
            sceneCount++;
        }
        if (dialogoCount == 7 && !Scene[2].active && sceneCount == 2) {
            Scene[2].SetActive(true);
            sceneCount++;
        }
       
    }
}
