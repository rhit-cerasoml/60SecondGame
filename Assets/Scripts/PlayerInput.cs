using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerInput : MonoBehaviour {

    // CONFIGS
    public float speed = 4.5f;
    public float jumpForce = 5.0f;
    public int JumpCount = 2;
    private int JumpsLeft = 0;
    public ForceMode2D jumpMode = ForceMode2D.Impulse;

    // CAMERA
    [SerializeField] Camera _camera;
    [SerializeField] float smoothLerpFactor;

    // PHYSICS
    private Rigidbody2D body;
    private CircleCollider2D _collider;

    // SOUND
    private AudioSource _audio_source;
    private bool _grounded = false;
    private bool _is_walking = false;
    private bool _is_walk_playing = false;

    [SerializeField] AudioClip sound_land;
    [SerializeField] AudioClip sound_jump;
    [SerializeField] AudioClip sound_walk;

    // ATTACK
    private GameObject _attack_collider;
    private bool _attack_on_cooldown = false;
    [SerializeField] float attack_cooldown;

    // Start is called before the first frame update
    void Start() {
        _camera = GameManager.Instance.getPlayerCamera();
        body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _audio_source = GetComponent<AudioSource>();
        _attack_collider = transform.Find("AttackCollider").gameObject;
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

        if(grounded && !_grounded){
            _audio_source.PlayOneShot(sound_land);
        }
        _grounded = grounded;


        Vector3 vel = body.velocity;
        if(grounded && vel.y == 0){
            JumpsLeft = JumpCount;
        }

        _is_walking = Mathf.Abs(deltaX) > 0.1f;

        if(_is_walking){
            float direction = Mathf.Sign(deltaX);
            updateDirection(direction > 0);
        }

        if(grounded && _is_walking && !_is_walk_playing){
            _is_walk_playing = true;
            StartCoroutine(WalkSound());
        }

        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && JumpsLeft > 0){
            vel.y = 0;
            body.velocity = vel;
            body.AddForce(Vector2.up * jumpForce, jumpMode);
            JumpsLeft--;
            _audio_source.PlayOneShot(sound_jump);
        }

        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !_attack_on_cooldown){
            onAttack();
        }
    }



    IEnumerator WalkSound() {
        while(_grounded && _is_walking){
            _audio_source.PlayOneShot(sound_walk);
            yield return new WaitForSeconds(0.3f + Random.Range(-0.1f, 0.1f));
        }
        _is_walk_playing = false;
    }

    IEnumerator AttackCooldown(){
        yield return new WaitForSeconds(attack_cooldown);
        _attack_on_cooldown = false;
    }

    void FixedUpdate(){
        Vector2 cameraFlatCoords = Vector2.Lerp(_camera.transform.position, transform.position, smoothLerpFactor);
        _camera.transform.position = new Vector3(
                cameraFlatCoords.x,
                cameraFlatCoords.y,
                -10
            );
    }

    void updateDirection(bool right_facing){
        if(right_facing){
            _attack_collider.transform.position = new Vector3(transform.position.x + 0.75f, transform.position.y, transform.position.z);
        }else{
            _attack_collider.transform.position = new Vector3(transform.position.x - 0.75f, transform.position.y, transform.position.z);
        }
    }

    void onAttack(){
        _attack_on_cooldown = true;
        StartCoroutine(AttackCooldown());
        Debug.Log("ATTACK!");

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        List<Collider2D> overlapColliders = new List<Collider2D>();

        _attack_collider.SetActive(true);
        int count = _attack_collider.GetComponent<BoxCollider2D>().OverlapCollider(filter, overlapColliders);
        Debug.Log(count);
        if(count != 0){
            foreach(Collider2D collider in overlapColliders){
                Debug.Log("HIT: " + collider);
                GameObject gameObject = collider.gameObject;
                if(gameObject.tag == "Enemy"){
                    Destroy(gameObject);
                    GameManager.Instance.IncrementScoreCount();
                }
            }
        }
        _attack_collider.SetActive(false);

    }
}
