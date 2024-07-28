using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    [SerializeField] private float offset;

    void Update() {
        HandleAiming();
    }

    void HandleAiming() {
        //Rotacao
        Vector2 posicaoAlvo = player.position;
        Vector2 posicaoAtual = transform.position;                                              
        float distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);                          
        Vector2 direcao = posicaoAlvo - posicaoAtual;
        var dir = direcao; 
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        //Girar
        Vector3 localScale = Vector3.one;

        if (angle > 90 || angle < -90) {
            localScale.y = -1f;
        }
        else {
            localScale.y = 1f;
        }

        transform.localScale = localScale;

    }
}