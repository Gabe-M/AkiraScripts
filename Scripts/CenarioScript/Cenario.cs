using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cenario : MonoBehaviour{
    private float lenght;
    private float startPos;

    private Transform cam;
    public float ParralaxEffect;
    public Transform playerpos;
    public float offset;

    void Start(){
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
        
    }


    void Update(){
        float Repos = cam.transform.position.x * (1 - ParralaxEffect);
        float Distance = cam.transform.position.x * ParralaxEffect;

        transform.position = new Vector3(startPos + Distance, playerpos.position.y + offset, transform.position.z);
        
        if(Repos > startPos + lenght * ParralaxEffect) {
            startPos += lenght ;
        }else if(Repos < startPos - lenght) {
            startPos -= lenght;
        }
    }
}
