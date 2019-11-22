using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GC : MonoBehaviour {

    enum BallCreationState { awaiting, created };

    // Events
    public delegate void GameEventHandler();
    public event GameEventHandler OnGameOver;
    public event GameEventHandler OnVictory;
    public event GameEventHandler OnScoreChange;
    public event GameEventHandler OnLivesChange;

    // singleton reference to the GameController
    public static GC main;

    // public attribute assigned in the editor
    public GameObject ballPrefab;
    public GameObject brickPrefab;

    // private attributes
    Paddle paddle;
    float halfHeight;
    float halfWidth;
    BallCreationState ballCreationState;
    float ballCreationTime;
    float ballCreationDelay;
    int livesRemaining;
    int score;
    int bricksRemaining;
    int maxBricks;

    // Public read-only properties
    public Paddle Paddle {
        get { return this.paddle; }
    }
    public float HalfHeight {
        get { return this.halfHeight; }
    }
    public float HalfWidth {
        get { return this.halfWidth; }
    }
    
    // public dual-access properties
    public int LivesRemaining {
        get { return this.livesRemaining; }
        set { 
            this.livesRemaining = value;
            this.OnLivesChange?.Invoke();
            if (this.livesRemaining <= 0){
                this.OnGameOver?.Invoke();
            }
        }
    }
    public int Score {
        get { return this.score; }
        set { 
            this.score = value;
            this.bricksRemaining = this.maxBricks - this.score;
            this.OnScoreChange?.Invoke();
            if (this.bricksRemaining <= 0){
                this.OnVictory?.Invoke();
            }
        }
    }

    void Awake() {
        if (GC.main == null){
            GC.main = this;
        }
        this.ballCreationDelay = 1;
        this.score = 0;
        this.livesRemaining = 5;
        this.CreateNewBall();
        this.CreateBricks();
        this.ballCreationTime -= this.ballCreationDelay;
        this.halfHeight = Camera.main.orthographicSize;
        this.halfWidth = this.halfHeight * Camera.main.aspect;
        this.paddle = GameObject.FindWithTag("Paddle").GetComponent<Paddle>();
    }

    void Start(){
        this.Score = this.score;
        this.LivesRemaining = this.livesRemaining;
    }

    void Update(){
        if (this.ballCreationState == BallCreationState.awaiting
         && this.ballCreationTime <= Time.time) {
            this.ballCreationState = BallCreationState.created;
            Instantiate(ballPrefab);
        }
    }

    public void CreateNewBall(){
        if (this.livesRemaining > 0){
            this.ballCreationState = BallCreationState.awaiting;
            this.ballCreationTime = Time.time + this.ballCreationDelay;
        }
    }

    public void CreateBricks(){
        this.bricksRemaining = 0;
        this.maxBricks = 0;
        float width, height, offset, yStart, x, y, rowHalfWidth;
        int row, col;
        Sprite brickSprite = this.brickPrefab.GetComponent<SpriteRenderer>().sprite;
        Vector2 brickDimensions = brickSprite.rect.size;
        width = brickDimensions.x * this.brickPrefab.transform.lossyScale.x / brickSprite.pixelsPerUnit;
        height = brickDimensions.y * this.brickPrefab.transform.lossyScale.y / brickSprite.pixelsPerUnit;
        offset = 0.1f * height;
        yStart = 3;
        for (row = 0; row < 8; row++){
            rowHalfWidth = 0.5f * row * (width + offset);
            for (col = 0; col <= row; col++){
                x = -rowHalfWidth + col * (offset + width);
                y = yStart - row * (offset + height);
                Instantiate(this.brickPrefab, new Vector3(x, y, 0), Quaternion.identity);
                this.bricksRemaining++;
            }
        }
        for (; row < 15; row++){
            rowHalfWidth = 0.5f * (14 - row) * (width + offset);
            for (col = 0; col <= 14 - row; col++){
                x = -rowHalfWidth + col * (offset + width);
                y = yStart - row * (offset + height);
                Instantiate(this.brickPrefab, new Vector3(x, y, 0), Quaternion.identity);
                this.bricksRemaining++;
            } 
        }
        this.maxBricks = this.bricksRemaining;
    }
}
