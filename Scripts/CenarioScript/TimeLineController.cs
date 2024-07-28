using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class TimeLineController : MonoBehaviour{

    [SerializeField] private Animator animator;
    public int Count;
    [SerializeField] private int CountMax;
    public PlayableDirector director;
    private RuntimeAnimatorController controller;
    private Dialogo1 dialogo1;
    private bool concertado;
    public int alvoCamera = 0;
    bool end;
    void Awake(){
        director = GetComponent<PlayableDirector>();
    }

      void OnEnable(){
        controller = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = null;
        director.Play();
    } 
    void Update() {
        if (director.state != PlayState.Playing) {
            End();
            gameObject.SetActive(false);
        }
    }
    void End() {
            animator.runtimeAnimatorController = controller;
            concertado = true;
            end = true;
        
    }
}
