using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBoundary : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Ball"){
            Destroy(other.gameObject);
            GC.main.LivesRemaining--;
            GC.main.CreateNewBall();
        }
    }

}
