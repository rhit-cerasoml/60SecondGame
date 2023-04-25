using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject player_prefab;

    [SerializeField] private int _score;
    [SerializeField] public TMP_Text _score_text;

    [SerializeField] Camera cameraReference;

    [SerializeField] public float time = 60.0f;
    [SerializeField] public TMP_Text _time_text;


    GameObject _player;


    public int Score_Count
    {
        get => _score;
        set
        {
            _score = value;
            _score_text.text = _score.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        _score = 0;
        //gameObject.GetComponent<AudioSource>().Play();
        _player = Instantiate(player_prefab);

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        _time_text.text = ((int) time).ToString();

        if (time <= 0.0f)
        {
            timerEnded();
        }
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public void IncrementScoreCount()
    {
        _score += 1;
        Debug.LogFormat("killed {0}", _score);
    }

    public void loadWalkAudio()
    {

    }

    public void loadDieAudio()
    {

    }

    public Camera getPlayerCamera(){
        return cameraReference;
    }

    public void timerEnded()
    {
        Debug.Log("Gameover");
    }
}
