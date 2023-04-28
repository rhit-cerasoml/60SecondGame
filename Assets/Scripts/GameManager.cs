using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager> {
    [SerializeField] GameObject player_prefab;
    [SerializeField] GameObject enemy_prefab;

    private int _score;
    [SerializeField] public TMP_Text _score_text;

    [SerializeField] Camera cameraReference;


    private float _time;
    [SerializeField] public TMP_Text _time_text;
    [SerializeField] public float run_time;

    GameObject _player;
    GameObject _enemy;


    public int Score_Count {
        get => _score;
        set {
            _score = value;
            _score_text.text = _score.ToString();
        }
    }

    // Start is called before the first frame update
    void Start() {
        Setup();
    }

    void Cleanup() {
        Destroy(_player);
        Destroy(_enemy);
        RoomManager.Instance.Cleanup();
    }

    void Setup() {
        _score = 0;
        _time = run_time;
        _player = Instantiate(player_prefab);
        _player.transform.position = new Vector3(7.5f, 2.5f, -1f);
        _enemy = Instantiate(enemy_prefab);
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

        if (_time <= 0.0f) {
            timerEnded();
        }
    }

    public GameObject GetPlayer() {
        return _player;
    }

    public void IncrementScoreCount() {
        _score += 1;
        Debug.LogFormat("killed {0}", _score);
    }

    public void loadWalkAudio() {

    }

    public void loadDieAudio() {

    }

    public Camera getPlayerCamera() {
        return cameraReference;
    }

    public void timerEnded() {
        Restart();
    }
}
