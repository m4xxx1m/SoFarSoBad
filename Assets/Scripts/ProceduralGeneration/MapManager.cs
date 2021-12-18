using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] public const int tilesBeyoundMap = 10;
    [SerializeField] public const int pixelsInTile = 100; //  оличество пикселей в одном блоке
    [SerializeField] public const int freeSpaceForSpawn = 5;

    [SerializeField] public CaveType caveType;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int[] chunkNumArray = new int[2];
    [SerializeField] private int seed;

    [SerializeField] private NoiseMapGenerator mapGenerator;
    [SerializeField] private NoiseMapRenderer mapRenderer;

    /*
     * 0 - враги
     * 1 - сундуки
     * 2 - детали
     * 3 - клетки с √рохогами
     */
    [SerializeField] private ItemsGenerator[] itemGenerators;
    [SerializeField] private int[] itemsCount = new int[4];

    public const int MAP_BORDER = 0;
    public const int WALL = 1;
    public const int FLOOR = 2;
    public const int ENEMIE = 3;
    public const int CHEST = 4;
    public const int GEAR = 5;
    public const int GROHOG_CELL = 6;
    public static readonly int[] ITEM_TYPES = new int[4] { ENEMIE, CHEST, GEAR, GROHOG_CELL };

    private void Start()
    {
        seed = GenerateSeed();
        for (int i = 0; i < chunkNumArray.Length; ++i)
            GenerateMap(chunkNumArray[i]);
        FreeSpaceForSpawn();
    }

    /*private void FixedUpdate()
    {
        offset.x -= 10;
        GenerateMap();
    }*/

    private int GenerateSeed()
    {
        return Random.Range(-10000000, 10000000);
    }

    public void SetCoordinates(Vector2 coordinates)
    {
        float x = coordinates.x;
        if (x > width * pixelsInTile)
        {
            if (x > (chunkNumArray[1] + 0.5f) * width * pixelsInTile)
            {
                RemoveMap(chunkNumArray[0]);
                chunkNumArray[0] = chunkNumArray[1];
                chunkNumArray[1] = chunkNumArray[1] + 1;
                GenerateMap(chunkNumArray[1]);
            }
            else if (x < (chunkNumArray[0] + 0.5f) * width * pixelsInTile)
            {
                RemoveMap(chunkNumArray[1]);
                chunkNumArray[1] = chunkNumArray[0];
                chunkNumArray[0] = chunkNumArray[0] - 1;
                GenerateMap(chunkNumArray[0]);
            }
        }
    }

    private void RemoveMap(int chunkNum)
    {
        mapRenderer.RemoveMap(width, height, chunkNum);
    }

    private void GenerateMap(int chunkNum)
    {
        Vector2 offset = new Vector2(chunkNum * width * pixelsInTile, 0);
        mapGenerator = new NoiseMapGenerator(width, height, new Vector2(offset.x / pixelsInTile, offset.y / pixelsInTile), seed);
        int[,] wallsMap = mapGenerator.GenerateNoiseMap();

        int[,] itemMap = new int[wallsMap.GetLength(0), wallsMap.GetLength(1)];
        if (caveType == CaveType.Grohog)
        {
            itemGenerators = new ItemsGenerator[2];
        }
        else
        {
            itemGenerators = new ItemsGenerator[4];
        }
        for (int i = 0; i < itemGenerators.Length; ++i)
        {
            itemGenerators[i] = new ItemsGenerator(width, height, ref wallsMap, itemsCount[i], ITEM_TYPES[i]);
            itemMap = itemGenerators[i].GenerateItemMap(itemMap);
        }

        mapRenderer = FindObjectOfType<NoiseMapRenderer>();
        mapRenderer.RenderMap(width, height, wallsMap, itemMap, offset);
    }

    private void FreeSpaceForSpawn()
    {
        mapRenderer.FreeSpaceForSpawn();
    }
}
