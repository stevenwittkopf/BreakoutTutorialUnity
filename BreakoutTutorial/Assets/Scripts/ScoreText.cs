using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    
    // private attributes
    Text uiBox;
    
    void Awake() {
        this.uiBox = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start() {
        GC.main.OnScoreChange += this.OnScoreChange;
    }

    void OnScoreChange() {
        this.uiBox.text = $"Score: {GC.main.Score}";
    }

}
