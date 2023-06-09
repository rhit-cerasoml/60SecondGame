using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager> {
    [SerializeField] GameObject player_prefab;

    private int _score;
    [SerializeField] public TMP_Text _score_text;

    [SerializeField] Camera cameraReference;


    private float _time;
    [SerializeField] public TMP_Text _time_text;
    [SerializeField] public float run_time;

    GameObject _player;

    public int Score_Count {
        get => _score;
        set {
            _score = value;
            _score_text.text = "Score: " + _score.ToString();
        }
    }

    // Start is called before the first frame update
    void Start() {
        Setup();
    }

    void Cleanup() {
        Destroy(_player);
        RoomManager.Instance.Cleanup();
    }

    void Setup() {
        _score = 0;
        _time = run_time;
        _player = Instantiate(player_prefab);
        _player.transform.position = new Vector3(7.5f, 2.5f, -1f);
        RoomManager.Instance.Setup();
    }

    void Restart() {
        Cleanup();
        Setup();
    }

    // Update is called once per frame
    void Update() {
        _time -= Time.deltaTime;
        _time_text.text = ((int) _time).ToString();

        //IncrementScoreCount();

        if (_time <= 0.0f) {
            timerEnded();
        }
    }

    public GameObject GetPlayer() {
        return _player;
    }

    public void IncrementScoreCount() {
        Score_Count += 1;
        Debug.LogFormat("killed {0}", _score);
    }

    public void DecrementScoreCount()
    {
        Score_Count -= 1;
        
        Debug.LogFormat("killed {0}", _score);
    }

    public Camera getPlayerCamera() {
        return cameraReference;
    }

    public void timerEnded() {
        Restart();
    }
}
