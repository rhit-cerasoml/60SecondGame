using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]

public class EnemyMovement : MonoBehaviour {
    public Transform Player;

    [SerializeField] float ray_angle;
    [SerializeField] float speed;
    bool _direction = true;

    private CircleCollider2D _collider;



    void Start() {
        _collider = GetComponent<CircleCollider2D>();
    }

    void Update() {
        
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, ray_angle) * Vector3.down, Color.green);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, -ray_angle) * Vector3.down, Color.green);
        
        if(_direction){
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int ray_hits = _collider.Raycast(Quaternion.Euler(0, 0, ray_angle) * Vector3.down, hits, 1.0f);
            if(ray_hits == 0){
                _direction = !_direction;
            }else{
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }else{
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int ray_hits = _collider.Raycast(Quaternion.Euler(0, 0, -ray_angle) * Vector3.down, hits, 1.0f);
            if(ray_hits == 0){
                _direction = !_direction;
            }else{
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }

    }
}
