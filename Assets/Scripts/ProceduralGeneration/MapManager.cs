using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] public const int tilesBeyoundMap = 10;
    [SerializeField] public const int pixelsInTile = 100; // Количество пикселей в одном блоке
    [SerializeField] public const int freeSpaceForSpawn = 5;

    [SerializeField] public CaveType caveType;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int[] chunkNumArray = new int[2];
    [SerializeField] private int seed;

    [SerializeField] private NoiseMapGenerator mapGenerator;
    private NoiseMapRenderer mapRenderer;

    public static MapManager GetInstance()
    {
        return instance;
    }

    /*
     * 0 - враги
     * 1 - сундуки
     * 2 - детали
     * 3 - клетки с Грохогами
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
        instance = this;
        Points.CurrentChunk = 1;
    }

    private void Awake()
    {
        mapRenderer = FindObjectOfType<NoiseMapRenderer>();
    }

    private int GenerateSeed()
    {
        return Random.Range(-10000000, 10000000);
    }

    public void SetCoordinates(Vector2 coordinates)
    {
        float x = coordinates.x;
        if (x > width)
        {
            if (x > (chunkNumArray[1] + 0.7f) * width)
            {
                RemoveMap(chunkNumArray[0]);
                chunkNumArray[0] = chunkNumArray[1];
                chunkNumArray[1] = chunkNumArray[1] + 1;
                itemsCount[0] += 1;
                if (caveType == CaveType.Vrudni)
                {
                    itemsCount[3] += (chunkNumArray[1] % 2 == 0 ? 1 : 0);
                }
                GenerateMap(chunkNumArray[1]);
                Points.CurrentChunk = chunkNumArray[1];
            }
            else if (x < (chunkNumArray[0] + 0.3f) * width)
            {
                RemoveMap(chunkNumArray[1]);
                chunkNumArray[1] = chunkNumArray[0];
                chunkNumArray[0] = chunkNumArray[0] - 1;
                GenerateMap(chunkNumArray[0], false);
                Points.CurrentChunk = chunkNumArray[1];
            }
        }
    }

    private void RemoveMap(int chunkNum)
    {
        mapRenderer.RemoveMap(width, height, chunkNum);
    }

    private void GenerateMap(int chunkNum, bool needItems = true)
    {
        Vector2 offset = new Vector2(chunkNum * width * pixelsInTile, 0);
        mapGenerator = new NoiseMapGenerator(width, height, new Vector2(chunkNum * width, 0), seed);
        int[,] wallsMap = mapGenerator.GenerateNoiseMap();
        int[,] itemMap = null;
        if (needItems)
        {
            itemMap = new int[wallsMap.GetLength(0), wallsMap.GetLength(1)];
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
        }
        
        mapRenderer.RenderMap(width, height, wallsMap, itemMap, offset);
    }

    private void FreeSpaceForSpawn()
    {
        mapRenderer.FreeSpaceForSpawn();
    }
}
