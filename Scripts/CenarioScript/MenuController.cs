using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour{
    
     [SerializeField] public Transform player;
    [SerializeField] public GameObject heartPack;
    [SerializeField] public GameObject buttonPack;
    [SerializeField] public GameObject numbersPack;
    [SerializeField] public Button startButtom;
     [SerializeField] public Button quitButtom;
     [SerializeField] public Button mapButtom;

     [SerializeField] public Button[] numbersButtom;
     
     [SerializeField] public Transform[] CheckPoints;
     
    void Awake(){
     startButtom.onClick.AddListener(StartGame);
     quitButtom.onClick.AddListener(QuitGame);
     mapButtom.onClick.AddListener(MapGame);
     

     numbersButtom[0].onClick.AddListener(One);
     numbersButtom[1].onClick.AddListener(Two);
     numbersButtom[2].onClick.AddListener(Three);

    }

    void StartGame(){
        gameObject.SetActive(false);
        heartPack.SetActive(true);
    }
    void QuitGame(){
        Application.Quit();
    }

    void MapGame(){
       buttonPack.SetActive(false);
       numbersPack.SetActive(true);
    }
    void One(){
        player.position= CheckPoints[0].position;
           gameObject.SetActive(false);
           heartPack.SetActive(true);
           
    }
    void Two(){
        player.position= CheckPoints[1].position;
           gameObject.SetActive(false);
           heartPack.SetActive(true);
    }
    void Three(){
        player.position= CheckPoints[2].position;
           gameObject.SetActive(false);
           heartPack.SetActive(true);
    }
}
