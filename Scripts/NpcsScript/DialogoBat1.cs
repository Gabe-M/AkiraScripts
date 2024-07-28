using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoBat1 : MonoBehaviour {
    public Sprite profile;
    #region speechText
    public string[] speechText1;
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
    private PlayerController playerCon;
    public bool startCene = false;
    private int sceneCount;
    [SerializeField] bool jaFalou = false;
    //public bool dialogoEnd;

    [SerializeField] bool onRadius;
    void Start() {
        dc = FindAnyObjectByType<DIalogoController>();


    }


    void Update() {
        Dialogo();
        dialogoCount = FindAnyObjectByType<DIalogoController>().DialogoCount;
        playerCon = FindAnyObjectByType<PlayerController>();
    }
    private void FixedUpdate() {
        Interact();
    }
    public void Interact() {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if (hit != null) {
            onRadius = true;
        }

        else { onRadius = false; }

    }
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void Dialogo() {
        if (onRadius && !jaFalou) {
             dc.SpeachNpc(profile, speechText1, actorName);
            jaFalou = true ;
        }
        if (Input.GetKeyDown(KeyCode.Return) && onRadius) {
            dc.NextSentence();

        }


    }
}