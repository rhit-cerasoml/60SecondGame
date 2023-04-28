using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTemplate : MonoBehaviour {

    [SerializeField] public int height;
    [SerializeField] public int width;

    [SerializeField] public int entrance_height;
    [SerializeField] public int exit_height;

    [SerializeField] public Vector2Int[] spawns;

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
                Vector3Int D_pos = new Vector3Int(x + x_offset, y + y_offset, 0);
                TileBase tile = foreground.GetTile(pos);
                if(tile != null){
                    D_foreground.SetTile(D_pos, tile);
                }
                tile = midground.GetTile(pos);
                if(tile != null){
                    D_midground.SetTile(D_pos, tile);
                }
                tile = background.GetTile(pos);
                if(tile != null){
                    D_background.SetTile(D_pos, tile);
                }
            }
        }
    }

    public void GenerateSpawns(int x_offset, int y_offset, GameObject[] prefabs, List<GameObject> enemies){
        for(int i = 0; i < spawns.Length; i++){
            Vector3 pos = new Vector3(x_offset + spawns[i].x - 0.5f, y_offset + spawns[i].y + 0.5f, 1.0f);
            GameObject enemy = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
            enemy.transform.position = pos;
            enemies.Add(enemy);
        }
    }
}
