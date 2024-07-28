using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    
public class ScriptFps : MonoBehaviour{

    private TextMeshProUGUI textMesh;

    void Start(){
        textMesh = GetComponent<TextMeshProUGUI>();

        InvokeRepeating(nameof(CalcularFPS), 0, 1);
        
    }

    void Update(){
        
    }

    private void CalcularFPS(){
        textMesh.text = (1f / UnityEngine.Time.deltaTime).ToString("00");
    }
}
