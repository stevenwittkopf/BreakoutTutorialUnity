using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeText : MonoBehaviour {

    // privateAttributes
    Text uiBox;

    void Awake() {
        this.uiBox = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start() {
        GC.main.OnGameOver += this.OnGameOver;
        GC.main.OnVictory += this.OnVictory;
    }

    // Update is called once per frame
    void OnGameOver() {
        this.uiBox.text = "GAME OVER!";
    }

    void OnVictory() {
        this.uiBox.text = "VICTORY!";
    }
}
