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
    float thrust;
    UpdateCall[] updates;

    public float Speed{
        get { return this.speed; }
    }

    // Called once upon object instantiation
    void Awake() {
        this.state = State.awaitingLaunch;
        this.rb = GetComponent<Rigidbody2D>();
        this.speed = 7.5f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        this.radius = this.transform.lossyScale.x * sr.sprite.rect.size.x / (2 * sr.sprite.pixelsPerUnit);
        this.updates = new UpdateCall[]{ this.AwaitingLaunchUpdate, this.InPlayUpdate };
        // The intended terminal velocity is 7.5; V = T/d, => T = V * d;
        this.thrust = 7.5f * this.rb.drag;
    }

    // Start is called before the first frame update
    void Start() {
        this.transform.position = GC.main.Paddle.transform.position + 0.5f * Vector3.up;
        GC.main.OnVictory += this.OnVictory;
    }

    // Update is called once per frame
    void FixedUpdate() {
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
        else {
            this.transform.position = GC.main.Paddle.transform.position + 0.5f * Vector3.up;
        }
    }

    void InPlayUpdate(){
        // Some thrust is applied in the direction of heading to ensure that the ball does not fall below a minimum velocity.
        // The linear drag coefficient of its RigidBody2D component will not allow it to exceed a maximum velocity
        this.rb.velocity += Time.fixedDeltaTime * this.thrust * this.rb.velocity.normalized;
    }

    void OnVictory(){
        this.gameObject.SetActive(false);
    }

    public void Reorient(){
        float direction;
        if (this.rb.velocity.x == 0){
            direction = 2 * Random.Range(0, 1) - 1;
        }
        else {
            direction = Mathf.Sign(this.rb.velocity.x); 
        }
        float angle = Mathf.PI * (0.5f - Mathf.Sign(direction)/3);
        this.rb.velocity = this.speed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

}
