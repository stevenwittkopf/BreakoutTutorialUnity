using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

    Rigidbody2D rb;
    float speed;
    
    void Awake(){
        this.rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start(){
        // Sets the speed of the paddle to be the halfWidth of the  screen.
        this.speed = GC.main.HalfWidth;
        GC.main.OnGameOver += this.OnGameOver;
        GC.main.OnVictory += this.OnVictory;
    }
    
    void FixedUpdate(){
        float direction = 0;
        if (Input.GetKey("left")){
            direction--;
        }
        if (Input.GetKey("right")){
            direction++;
        }
        rb.velocity = direction * this.speed * Vector2.right;
    }

    void OnGameOver(){
        // Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    void OnVictory(){
        this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision){
        GameObject go = collision.collider.gameObject;
        if (go.tag == "Ball"){
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            // If the ball is not moving fast enough horizontally, impart horizontal velocity.
            float speed = rb.velocity.magnitude;
            if (rb.velocity.x < 0 && go.transform.position.x > this.transform.position.x
             || rb.velocity.x > 0 && go.transform.position.x < this.transform.position.x){
                rb.velocity -= 2 * rb.velocity.x * Vector2.right;
            }
            if (Mathf.Abs(rb.velocity.x) < 0.5f){
                // If the ball is on the right side of the screen go left
                if (go.transform.position.x > 0){
                    rb.velocity += Vector2.left;
                }
                // If the ball is on the left side of the screen go right
                else {
                    rb.velocity += Vector2.right;
                }
                // After altering the direction, original speed is maintained.
                rb.velocity = speed * rb.velocity.normalized;
            }
            // If the ball is not moving fast enough vertically impart vertical velocity;
            if (Mathf.Abs(rb.velocity.y) < 0.5f){
                rb.velocity += 5 * Vector2.up;
                rb.velocity = speed * rb.velocity.normalized;
            }
            // Flip x-velocity of the ball when it hits the right side of the paddle and is going left
            // OR when it hits the left side of the paddle and is going right
            // Last check if the speed is below the minimum speed threshold
            if (speed < 2){
                rb.velocity = 2 * rb.velocity.normalized;
            }
        }
    }
}
