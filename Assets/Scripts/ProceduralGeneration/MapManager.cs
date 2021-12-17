using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] public static readonly int tilesBeyoundMap = 5;
    [SerializeField] public static readonly int pixelsInTile = 100; // Количество пикселей в одном блоке

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int seed;

    [SerializeField] private NoiseMapGenerator mapGenerator;
    [SerializeField] private NoiseMapRenderer mapRenderer;

    private void Start()
    {
        GenerateMap();
    }

    /*private void Update()
    {
        GenerateMap();
    }*/

    public void SetCoordinates(Vector2 coordinates)
    {
        if (Mathf.Abs(coordinates.x - offset.x) >= width / 5 * 3 * pixelsInTile)
        {
            offset = coordinates;
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        mapGenerator = new NoiseMapGenerator(width, height, new Vector2(offset.x / pixelsInTile, offset.y / pixelsInTile), seed);
        float[,] wallsMap = mapGenerator.GenerateNoiseMap();

        mapRenderer = FindObjectOfType<NoiseMapRenderer>();
        mapRenderer.RenderMap(width, height, wallsMap, offset);
    }
}
