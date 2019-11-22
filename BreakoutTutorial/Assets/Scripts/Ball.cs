using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    enum State { awaitingLaunch, inPlay };
    delegate void UpdateCall();

    State state;
    Rigidbody2D rb;
    float speed;
    float radius;
    UpdateCall[] updates;

    // Called once upon object instantiation
    void Awake() {
        this.state = State.awaitingLaunch;
        this.rb = GetComponent<Rigidbody2D>();
        this.speed = 7.5f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        this.radius = this.transform.lossyScale.x * sr.sprite.rect.size.x / (2 * sr.sprite.pixelsPerUnit);
        this.updates = new UpdateCall[]{ this.AwaitingLaunchUpdate, this.InPlayUpdate };
    }

    // Start is called before the first frame update
    void Start() {
        this.transform.position = GC.main.Paddle.transform.position + 0.5f * Vector3.up;
        GC.main.OnVictory += this.OnVictory;
    }

    // Update is called once per frame
    void Update() {
        // switch (this.state){
        //     case State.awaitingLaunch:
        //         this.AwaitingLaunchUpdate();
        //         break;
        //     case State.inPlay:
        //         this.InPlayUpdate();
        //         break;
        // }
        // The state-dependent array access of function calls imitates a switch
        this.updates[(int)this.state]();
    }

    void AwaitingLaunchUpdate(){
        this.transform.position = GC.main.Paddle.transform.position + 0.5f * Vector3.up;
        if (Input.GetKeyDown("space")) {
            this.state = State.inPlay;
            // The launch angle is determined by the direction of the paddle. 
            // If the paddle is not moving a direction is chosen at random
            float direction = 0;
            if (Input.GetKey("left")){
                direction--;
            }
            if (Input.GetKey("right")){
                direction++;
            }
            if (direction == 0) {
                direction = 2 * Random.Range(0, 2) - 1;
            }
            float angle = Mathf.PI * (0.5f - direction/3f);
            this.rb.velocity = this.speed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    void InPlayUpdate(){
        // Intended No-Op: 2D Physics Engine handles movement and collisions
    }

    void OnVictory(){
        this.gameObject.SetActive(false);
    }

}
