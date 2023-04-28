using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>{

    [SerializeField] GameObject startingRoom;

    void Start() {
        Transform grid = transform.GetChild(0);
        startingRoom.transform.GetComponent<RoomTemplate>().Stamp(0, 0, grid);
    }

    void Update() {
        
    }
}
