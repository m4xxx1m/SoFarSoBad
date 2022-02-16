using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapRenderer : MonoBehaviour
{
    [SerializeField] private Tile borderTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile enemieTile;
    [SerializeField] private Tile chestTile;
    [SerializeField] private Tile gearTile;
    [SerializeField] private Tile grohogCellTile;

    private Tilemap tilemap;

    public void RenderMap(int width, int height, int[,] wallsMap, int[,] itemMap, Vector2 coordinates)
    {
        ApplyMap(width, height, wallsMap, itemMap, coordinates);
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void ApplyMap(int width, int height, int[,] map, int[,] itemMap, Vector2 coordinates)
    {
        //tilemap = GetComponent<Tilemap>();
        tilemap.size = new Vector3Int(map.GetLength(0), map.GetLength(1), 0);
        for (int y = 0; y < map.GetLength(0); ++y)
        {
            for (int x = 0; x < map.GetLength(1); ++x)
            {
                int tbm = MapManager.tilesBeyoundMap;
                int pit = MapManager.pixelsInTile;

                switch (map[y, x])
                {
                    case MapManager.MAP_BORDER:
                        tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, 0), borderTile);
                        break;
                    case MapManager.WALL:
                        tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, 0), wallTile);
                        break;
                    case MapManager.FLOOR:
                        tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, -1), floorTile);
                        break;
                }
                
                if (itemMap != null)
                {
                    switch (itemMap[y, x])
                    {
                        case MapManager.ENEMIE:
                            tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, 0), enemieTile);
                            break;
                        case MapManager.CHEST:
                            tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, 0), chestTile);
                            break;
                        case MapManager.GEAR:
                            tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, 0), gearTile);
                            break;
                        case MapManager.GROHOG_CELL:
                            tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - 2, y - height / 2 - tbm, 0), grohogCellTile);
                            break;
                    }
                }
            }
        }
        tilemap.CompressBounds();
    }

    public void RemoveMap(int width, int height, int chunkNum)
    {
        int tbm = MapManager.tilesBeyoundMap;
        //int pit = MapManager.pixelsInTile;
        //tilemap = GetComponent<Tilemap>();
        for (int y = -tbm; y < height + tbm; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                tilemap.SetTile(new Vector3Int(chunkNum * width + x - 2, y - height / 2, 0), null);
                tilemap.SetTile(new Vector3Int(chunkNum * width + x - 2, y - height / 2, -1), null);
            }
        }
    }

    public void FreeSpaceForSpawn()
    {
        //tilemap = GetComponent<Tilemap>();
        int fsfs = MapManager.freeSpaceForSpawn;
        for (int y = -fsfs; y < fsfs; ++y)
        {
            for (int x = -2; x < fsfs*2 - 2; ++x)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
                tilemap.SetTile(new Vector3Int(x, y, -1), floorTile);
            }
        }
    }
}
