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
        this.gameObject.SetActive(false);
    }

    void OnVictory(){
        this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision){
        GameObject go = collision.collider.gameObject;
        if (go.tag == "Ball"){
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            Ball ball = go.GetComponent<Ball>();
            // If the ball is not moving fast enough vertically or horizontally, reorient.
            if (Mathf.Abs(rb.velocity.x) <0.5f * ball.Speed || Mathf.Abs(rb.velocity.y) < 0.5f * ball.Speed){
                ball.Reorient();
            }
            // If the ball is going left and hits the right side of the paddle
            // or if the ball is going right and hits the left side of the paddle
            // Flip its x-velocity
            if (rb.velocity.x < 0 && ball.transform.position.x > this.transform.position.x
             || rb.velocity.x > 0 && ball.transform.position.x < this.transform.position.x){
                rb.velocity -= 2 * rb.velocity.x * Vector2.right;
            }
        }
    }
}
