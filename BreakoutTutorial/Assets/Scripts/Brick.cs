using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    enum CollisionState { active, inactive }
    // Static attributes and their allocation
    static Sprite[] sprites;

    // private attributes
    int hitPoints;
    SpriteRenderer sr;
    float collisionTime;
    float collisionDelay;
    CollisionState collisionState;
    BoxCollider2D bc; 

    void Awake(){
        if (Brick.sprites == null){
            string[] spriteNames = new string[]{ "GreenSquare", "BlueSquare", "RedSquare" };
            Brick.sprites = new Sprite[spriteNames.Length];
            for (int i = 0; i < spriteNames.Length; i++) {
                Brick.sprites[i] = Resources.Load<Sprite>($"Sprites/{spriteNames[i]}");
            }
        }
        this.hitPoints = 3;
        this.sr = GetComponent<SpriteRenderer>();
        this.sr.sprite = Brick.sprites[this.hitPoints - 1];
        this.collisionDelay = 0.25f;
        this.collisionState = CollisionState.active;
        this.bc = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate(){
        if (this.collisionState == CollisionState.inactive
         && this.collisionTime >= Time.time){
            this.collisionState = CollisionState.active;
            this.bc.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.gameObject.tag == "Ball"){
            this.hitPoints--;
            if (this.hitPoints <= 0){
                this.gameObject.SetActive(false);
                GC.main.Score++;
            }
            else {
                this.sr.sprite = Brick.sprites[this.hitPoints - 1];
                this.bc.enabled = false;
                this.collisionState = CollisionState.inactive;
                this.collisionTime = Time.time + this.collisionDelay;
            }
        }
    }

}
