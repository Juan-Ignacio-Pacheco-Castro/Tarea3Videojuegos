using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private GameObject _ball;
    private Transform _ballTransform;
    // Start is called before the first frame update
    [SerializeField] Pad Paddle;

    private Rigidbody2D _ballRigidbody;

    private bool _playing = false;
    private Vector2 _paddleToBallVector;

    [SerializeField] private float xVelocity;
    [SerializeField] private float yVelocity;
    private float totalVelocity;

    void Start()
    {
        _ball = this.gameObject;
        _ballTransform = this.gameObject.transform;
        _paddleToBallVector = transform.position - Paddle.transform.position;
        _ballRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        totalVelocity = Mathf.Abs(xVelocity) + Mathf.Abs(yVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        LockToPaddle();
        LaunchOnClick();
    }

    void LockToPaddle(){
        if(!_playing){
            Vector2 paddleRef = Paddle.transform.position;
            Vector2 paddlePos = new Vector2(paddleRef.x, paddleRef.y);
            transform.position = paddlePos + _paddleToBallVector;
        }
    }

    void LaunchOnClick(){
        if(Input.GetMouseButtonDown(0)){
            _playing = true;
            _ballRigidbody.velocity = new Vector2(xVelocity, yVelocity);
        }
    }

    void OnPlayerLost(){
        //GameManager.instance.Lives = GameManager.instance.Lives - 1; 
        GameManager.instance.UpdateLives(GameManager.instance.Lives - 1);
        _playing = false;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Paddle"){
            BouncePaddle(collision);
        } else if (collision.gameObject.tag == "Block") {
            BounceBlock(collision);
        } else if (collision.gameObject.tag == "Vertical Wall") {
            BounceVertical();
        } else if (collision.gameObject.tag == "Horizontal Wall") {
            BounceHorizontal();
        }
    }

    void BouncePaddle(Collision2D collision) {
        if (CheckCenterPaddle(collision)) {
            _ballRigidbody.velocity = new Vector2(0, totalVelocity);
        } else {
            float newXVelocity = CalculateXVelocity(collision.gameObject.transform.position.x, _ballTransform.position.x);
            float newYVelocity = totalVelocity - Mathf.Abs(newXVelocity);
            _ballRigidbody.velocity = new Vector2(newXVelocity, newYVelocity);
        }
    }

    bool CheckCenterPaddle(Collision2D collision) {
        return ((collision.gameObject.transform.position.x - (0.1 * collision.gameObject.transform.localScale.x)) < _ballTransform.position.x) && ((collision.gameObject.transform.position.x + (0.1 * collision.gameObject.transform.localScale.x)) > _ballTransform.position.x);
    }

    float CalculateXVelocity(float paddlePosition, float ballPosition){
        float relativePosition = ballPosition - paddlePosition;
        return relativePosition * 3;
    }

    void BounceVertical(){
        _ballRigidbody.velocity = new Vector2(-_ballRigidbody.velocity.x, _ballRigidbody.velocity.y);
    }

    void BounceHorizontal(){
        _ballRigidbody.velocity = new Vector2(_ballRigidbody.velocity.x, -_ballRigidbody.velocity.y);
    }

    void BounceBlock(Collision2D collision) {
        if (Mathf.Abs(_ballTransform.position.x - collision.gameObject.transform.position.x) > Mathf.Abs(_ballTransform.position.y - collision.gameObject.transform.position.y) ) {
            BounceVertical();
        } else {
            BounceHorizontal();
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        Debug.Log(collider.tag);
        if(collider.tag == Constants.LOST){
            OnPlayerLost();
        }   
    }
}
