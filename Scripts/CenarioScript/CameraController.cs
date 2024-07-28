using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private Transform[] target;
    [SerializeField]
    private Vector2 targetOffset;
    public int i = 0;
    public bool startCene = false;
    [SerializeField] private GameObject[] scene;

    private void Update() {
        
            if (scene[0].activeSelf == true || scene[1].activeSelf == true || scene[2].activeSelf == true) { 
                i = 1;
            }
            else {
                i = 0;
            }
    }
    


    private void LateUpdate() {
        Vector3 mouseScreenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); //Detecta a posição do mouse na tela

            
        mouseScreenPosition.x = (mouseScreenPosition.x - 0.5f) * 2f;
        mouseScreenPosition.y = (mouseScreenPosition.y - 0.5f) * 2f;

        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, -1f, 1f);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, -1f, 1f);

        float offsetX = targetOffset.x * mouseScreenPosition.x;
        float offsetY = targetOffset.y * mouseScreenPosition.y;

        Vector3 cameraPosition = new Vector3(target[i].position.x + offsetX, target[i].position.y + offsetY, transform.position.z);

        if (offsetX > -10 && offsetX < 10) {
            transform.position = new Vector3(target[i].position.x, target[i].position.y, transform.position.z);
        }
        if (offsetY > -1.9f && offsetY < 1.9f) {
            transform.position = new Vector3(target[i].position.x, target[i].position.y, transform.position.z);
        }
        if (offsetY >= 1.9f) {
            transform.position = cameraPosition;

        }
        else if (offsetY <= -1.9f) {
            transform.position = cameraPosition;

        }
        if (offsetX >= 10) {
            transform.position = cameraPosition;
        
        }else if (offsetX <= -10f) {
            transform.position = cameraPosition;

        }

       // Debug.Log(offsetY);

    }
}