using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPlayer : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;
        if (GameManager.Instance.GetPlayer() == other) {
            // HERE we know that the other object we collided with is an enemy
            GameManager.Instance.DecrementScoreCount();
        }
    }   
}
