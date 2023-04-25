using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public float speed = 4.5f;
    public float jumpForce = 5.0f;
    public int JumpCount = 2;
    private int JumpsLeft = 0;
    public ForceMode2D jumpMode = ForceMode2D.Impulse;

    [SerializeField] Camera _camera;
    [SerializeField] float smoothLerpFactor;

    private Rigidbody2D body;
    private CircleCollider2D _collider;

    // Start is called before the first frame update
    void Start() {
        _camera = GameManager.Instance.getPlayerCamera();
        body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        
    }

    // Update is called once per frame
    void Update() {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;
        
        Vector3 minBound = _collider.bounds.min;
        Vector3 maxBound = _collider.bounds.max;

        Vector2 bot_right = new Vector2(minBound.x - 0.1f, minBound.y - 0.1f);
        Vector2 bot_left = new Vector2(maxBound.x - 0.1f, minBound.y - 0.1f);

        Collider2D hit = Physics2D.OverlapArea(bot_left, bot_right);

        bool grounded = hit != null;

        Vector3 vel = body.velocity;
        if(grounded && vel.y == 0){
            JumpsLeft = JumpCount;
        }

        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && JumpsLeft > 0){
            vel.y = 0;
            body.velocity = vel;
            body.AddForce(Vector2.up * jumpForce, jumpMode);
            JumpsLeft--;
        }
    }

    void FixedUpdate(){
        Vector2 cameraFlatCoords = Vector2.Lerp(_camera.transform.position, transform.position, smoothLerpFactor);
        _camera.transform.position = new Vector3(
                cameraFlatCoords.x,
                cameraFlatCoords.y,
                -10
            );
    }
}
