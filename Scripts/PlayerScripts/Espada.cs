using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour{
    [SerializeField] private float tempo;
    public int dano;
    public Enemy enemy;
    private void Awake() {    }
    void Start(){
        Destroy(gameObject, tempo);
    }

    void Update(){
        
    }
    
}
