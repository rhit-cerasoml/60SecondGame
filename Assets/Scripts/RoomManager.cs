using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>{

    [SerializeField] GameObject startingRoom;
    [SerializeField] GameObject mapPrefab;
    [SerializeField] int room_count;
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] GameObject[] enemyPrefabs;
    

    GameObject _map;
    List<GameObject> _enemy_list;

    void Start() {
        
    }

    public void Cleanup(){
        Destroy(_map);
        foreach(GameObject enemy in _enemy_list){
            Destroy(enemy);
        }
    }

    public void Setup(){
        _enemy_list = new List<GameObject>();
        _map = Instantiate(mapPrefab);
        _map.transform.position = new Vector3(0, 0, 0);
        RoomTemplate room_template = startingRoom.transform.GetComponent<RoomTemplate>();
        room_template.Stamp(0, 0, _map.transform);
        int current_height = room_template.exit_height;
        int current_left_edge = room_template.width;
        for(int i = 0; i < room_count; i++){
            int room_num = Random.Range(0, roomPrefabs.Length);
            room_template = roomPrefabs[room_num].transform.GetComponent<RoomTemplate>();
            current_height -= room_template.entrance_height;
            Debug.Log("Stamping at " + current_left_edge + " and height " + current_height);
            room_template.Stamp(current_left_edge, current_height, _map.transform);
            room_template.GenerateSpawns(current_left_edge, current_height, enemyPrefabs, _enemy_list);
            current_height += room_template.exit_height;
            current_left_edge += room_template.width;
        }
    }

    void Update() {
        
    }
}
