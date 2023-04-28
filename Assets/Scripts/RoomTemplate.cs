using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTemplate : MonoBehaviour {

    [SerializeField] int height;
    [SerializeField] int width;

    [SerializeField] int entrance_height;
    [SerializeField] int exit_height;
    
    
    void Start() {
        
    }

    void Update() {
        
    }

    public void Stamp(int x_offset, int y_offset, Transform destination){
        Transform grid = transform.GetChild(0);
        Tilemap foreground = grid.GetChild(0).gameObject.GetComponent<Tilemap>();
        Tilemap midground = grid.GetChild(1).gameObject.GetComponent<Tilemap>();
        Tilemap background = grid.GetChild(2).gameObject.GetComponent<Tilemap>();
        
        Tilemap D_foreground = destination.GetChild(0).gameObject.GetComponent<Tilemap>();
        Tilemap D_midground = destination.GetChild(1).gameObject.GetComponent<Tilemap>();
        Tilemap D_background = destination.GetChild(2).gameObject.GetComponent<Tilemap>();

        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = foreground.GetTile(pos);
                if(tile != null){
                    D_foreground.SetTile(pos, tile);
                }
                tile = midground.GetTile(pos);
                if(tile != null){
                    D_midground.SetTile(pos, tile);
                }
                tile = background.GetTile(pos);
                if(tile != null){
                    D_background.SetTile(pos, tile);
                }
            }
        }
    }
}
