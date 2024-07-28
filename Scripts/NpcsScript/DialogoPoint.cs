using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoPoint : MonoBehaviour{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask npcLayer;

    void Start(){
        
    }


    void Update(){
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, npcLayer);
       
    }
}
