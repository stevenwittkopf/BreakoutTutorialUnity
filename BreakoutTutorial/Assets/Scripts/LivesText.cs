using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour {

    // Private attributes
    Text uiBox;

    void Awake() {
        this.uiBox = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start() {
        GC.main.OnLivesChange += this.OnLivesChange;
    }

    void OnLivesChange() {
        this.uiBox.text = $"Lives: {GC.main.LivesRemaining}";
    }
}
