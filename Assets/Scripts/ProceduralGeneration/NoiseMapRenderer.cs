using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapRenderer : MonoBehaviour
{
    // [SerializeField] public SpriteRenderer spriteRenderer = null;
    [SerializeField] private float level = 0.5f; // от 0 до 1, значени€ больше этого уровн€ создают блок
    [SerializeField] private Tile borderTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile floorTile;
    private Tilemap tilemap;

    private const int MAP_BORDER = 0;
    private const int WALL = 1;
    private const int FLOOR = 2;

    public void RenderMap(int width, int height, float[,] noiseMap, Vector2 coordinates)
    {
        ApplyMap(width, height, GenerateNoiseMap(width, height, noiseMap), coordinates);
    }

    private void ApplyMap(int width, int height, int[,] map, Vector2 coordinates)
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.size = new Vector3Int(map.GetLength(0), map.GetLength(1), 0);
        for (int y = 0; y < map.GetLength(0); ++y)
        {
            for (int x = 0; x < map.GetLength(1); ++x)
            {
                // Debug.Log($"{y}, {x}, {map[y, x]}");
                int tbm = MapManager.tilesBeyoundMap;
                int pit = MapManager.pixelsInTile;
                switch (map[y, x])
                {
                    case MAP_BORDER:
                        tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - tbm - 2, y - height / 2 - tbm, 0), borderTile);
                        break;
                    case WALL:
                        tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - tbm - 2, y - height / 2 - tbm, 0), wallTile);
                        break;
                    case FLOOR:
                        tilemap.SetTile(new Vector3Int(x + (int)(coordinates.x / pit) - tbm - 2, y - height / 2 - tbm, 0), floorTile);
                        break;
                }
            }
        }
        //Texture2D texture = new Texture2D(width/**pixelsInBlock*/, height/**pixelsInBlock*/);
        /*texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colors);
        texture.Apply();

        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);*/
    }

    // ѕреобразуем массив с данными о шуме в массив чЄрно-белых цветов, дл€ передачи в текстуру
    private int[,] GenerateNoiseMap(int width, int height, float[,] noiseMap)
    {
        /*Color[] colorMap = new Color[noiseMap.Length * pixelsInBlock * pixelsInBlock];
        for (int i = 0; i < noiseMap.Length; i++)
        {
            Color color;
            if (noiseMap[i] < level)
            {
                color = Color.white;
            }
            else
            {
                color = Color.black;
            }
            colorMap[i] = color;
            for (int j = 0; j < pixelsInBlock; ++j)
            {
                for (int k = 0; k < pixelsInBlock; ++k)
                {
                    colorMap[(i % width) * pixelsInBlock + k + j * width * pixelsInBlock + (i / width) * pixelsInBlock * width * pixelsInBlock] = color;
                }
            }
        }*/
        int tbm = MapManager.tilesBeyoundMap;
        int[,] tileMap = new int[height + 2 * tbm, width + 2 * tbm];
        for (int i = 0; i < height + 2 * tbm; ++i)
        {
            for (int j = 0; j < width + 2*tbm; ++j)
            {
                if (noiseMap[i, j] == -1.0f)
                {
                    tileMap[i, j] = MAP_BORDER;
                }
                else if (noiseMap[i, j] >= level)
                {
                    tileMap[i, j] = WALL;
                }
                else
                {
                    tileMap[i, j] = FLOOR;
                }
            }
        }
        return tileMap;
    }
}
