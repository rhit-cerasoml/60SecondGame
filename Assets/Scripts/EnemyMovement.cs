using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum MoveType { Left, Right, Random, Seek };
    [SerializeField] private MoveType _current_move_type;

    public float speed = 2f;

    private CircleCollider2D _collider;
    private int _left_ray_hits;
    private int _right_ray_hits;

    public float ray_rotation = 30;


    //[SerializeField] private GameManager _manager;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        //EventBus.Subscribe(EventBus.EventType.KillsUpdate, Jump);
    }

    private void OnDestroy()
    {
        //EventBus.Unsubscribe(EventBus.EventType.KillsUpdate, Jump);
    }

    // Update is called once per frame
    void Update()
    {
        //for debugging and showing what the third parameter to Quaternion.Euler does
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, ray_rotation) * Vector3.down, Color.white);

        //drawy the rays we'll use in the WanderLeft() and WanderRight() methods
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, -45) * Vector3.down, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, 45) * Vector3.down, Color.green);

        //do the actual movement
        MoveType next_move_type = _current_move_type;//add this later after you make them swap direction
        switch (_current_move_type)
        {
            case MoveType.Left:
                next_move_type = WanderLeft();
                break;
            case MoveType.Right:
                next_move_type = WanderRight();
                break;
            case MoveType.Random:
                WanderRandom();
                break;
            case MoveType.Seek:
                Seek();
                break;
        }
        _current_move_type = next_move_type;

    }

    //could do this with just one method that takes a direction
    //woudl require a little refactoring of the raycasts
    MoveType WanderLeft()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //raycast to the left to see if we are near an edge
        //if we return no hits that means there is empty space over there
        //last parameter is how far to cast, using 1 here, since that is about the diameter of our enemy
        //the hits argument is the array that lists the things we hit, we don't use this here, but its needed
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int ray_hits = _collider.Raycast(Quaternion.Euler(0, 0, -45) * Vector3.down, hits, 1.0f);
        if (ray_hits == 0)
        {
            //about to walk off an edge, swap directions!
            return MoveType.Right;
        }
        return MoveType.Left;
    }

    MoveType WanderRight()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int ray_hits = _collider.Raycast(Quaternion.Euler(0, 0, 45) * Vector3.down, hits, 1.0f);
        if (ray_hits == 0)
        {
            //about to walk off an edge, swap directions!
            return MoveType.Left;
        }
        return MoveType.Right;
    }

    private bool _finished_wander = true;
    void WanderRandom()
    {
        if (_finished_wander)
        {
            if (Random.Range(0f, 1f) < 0.5)
            {
                StartCoroutine(WanderCoroutine(MoveType.Left));
            }
            else
            {
                StartCoroutine(WanderCoroutine(MoveType.Right));

            }
        }

    }

    public float vision_distance = 2.5f;
    public float jump_height = 2f;
    void Seek()
    {
        float player_distance = Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position);
        //Debug.LogFormat("Player is {0} units away", player_distance);
        if (player_distance <= vision_distance)
        {
            bool los = LineOfSight();
            if (!los)//stop chasing if I lose line of sight
                return;
            Vector3 direction_to_player = GameManager.Instance.GetPlayer().transform.position - transform.position;
            Vector3 movement = direction_to_player.normalized;//using this directly makes the enemy kinda bounce towards you and jump under you
            //this is smoother, no bouncing = less fun, but whatevs
            if (direction_to_player.x > 0)
                movement = new Vector3(1, 0, 0);
            else
                movement = new Vector3(-1, 0, 0);
            // do we want them to try and jump if they are directly below us?
            if (Mathf.Abs(direction_to_player.x) < 0.1)//(Mathf.Approximately(0f, direction_to_player.x))
            {
                //Debug.Log("Player is above me!");
                movement.y = jump_height;
            }
            transform.Translate(movement * speed * Time.deltaTime);
        }


    }

    bool LineOfSight()
    {
        Vector3 direction_to_player = GameManager.Instance.GetPlayer().transform.position - transform.position;
        RaycastHit2D hits = Physics2D.Raycast(transform.position, direction_to_player);

        //Debug.LogFormat("I see {0}", hits.transform.name);
        PlayerInfo pi = hits.transform.GetComponent<PlayerInfo>();
        if (pi == null)
        {
            //Debug.Log("Not a player!");
            return false;
        }
        return true;
    }


    private IEnumerator WanderCoroutine(MoveType direction)
    {
        float wander_time = 2f;
        _finished_wander = false;
        Debug.Log("Starting Wander towards " + direction.ToString());
        while (wander_time > 0)
        {
            if (direction == MoveType.Left)
                direction = WanderLeft();
            else
                direction = WanderRight();

            wander_time -= Time.deltaTime;
            yield return null;
        }

        Debug.Log("Finished Wander...");
        _finished_wander = true;
    }

    private void Jump()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.AddForce(Vector3.up * 3f, ForceMode2D.Impulse);
    }
}
