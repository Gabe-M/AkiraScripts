using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour{
    

    void Start(){
        Destroy(gameObject, 1);
    }

    void Update(){
       
    }

    /*void Espada() {
      var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
      var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    
 }

 private void OnDrawGizmos() {
     //  Gizmos.DrawWireCube(attackPoint.position, new Vector2(3.5f,1.5f));
     Gizmos.DrawWireSphere(attackPoint.position, radius);
 }
*/
}
