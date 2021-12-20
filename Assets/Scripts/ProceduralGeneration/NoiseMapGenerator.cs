using UnityEngine;

public class NoiseMapGenerator
{
    private int width;
    private int height;
    private Vector2 offset;

    private float scale;
    private int octaves;
    private float persistence;
    private float lacunarity;
    private int seed;

    private float level = 0.5f; // от 0 до 1, значения больше этого уровня создают блок
    private float level2 = 0.85f; // блоки после этого значения не должны разрушаться

    public NoiseMapGenerator(int w, int h, Vector2 o, int s): this(w, h, o, 15f, 4, 0.5f, 2f, s)
    { }

    public NoiseMapGenerator(int w, int h, Vector2 o, float sc, int oct, float p, float l, int s)
    {
        width = w;
        height = h;
        scale = sc;
        octaves = oct;
        persistence = p;
        lacunarity = l;
        seed = s;
        offset = o;
    }

    public int[,] GenerateNoiseMap()
    {
        // Массив данных о вершинах, одномерный вид поможет избавиться от лишних циклов впоследствии
        float[] noiseMap = new float[width * height];

        // Порождающий элемент
        System.Random rand = new System.Random(seed);

        // Сдвиг октав, чтобы при наложении друг на друга получить более интересную картинку
        Vector2[] octavesOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            // Учитываем внешний сдвиг положения
            float xOffset = rand.Next(-100000, 100000);
            float yOffset = rand.Next(-100000, 100000);
            octavesOffset[i] = new Vector2(xOffset / width, yOffset / height);
        }

        if (scale < 0)
        {
            scale = 0.0001f;
        }

        // Учитываем половину ширины и высоты, для более визуально приятного изменения масштаба
        float halfWidth = width / 2f;
        float halfHeight =  height / 2f;

        // Генерируем точки на карте высот
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Задаём значения для первой октавы
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                float superpositionCompensation = 0;

                // Обработка наложения октав
                for (int i = 0; i < octaves; i++)
                {
                    // Рассчитываем координаты для получения значения из Шума Перлина
                    float xResult = (x - halfWidth + offset.x) / scale * frequency + octavesOffset[i].x * frequency;
                    float yResult = (y - halfHeight) / scale * frequency + octavesOffset[i].y * frequency;

                    // Получение высоты из ГСПЧ
                    float generatedValue = Mathf.PerlinNoise(xResult, yResult);
                    // Наложение октав
                    noiseHeight += generatedValue * amplitude;
                    // Компенсируем наложение октав, чтобы остаться в границах диапазона [0,1]
                    noiseHeight -= superpositionCompensation;

                    // Расчёт амплитуды, частоты и компенсации для следующей октавы
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    superpositionCompensation = amplitude / 2;
                }

                // Сохраняем точку для карты высот
                // Из-за наложения октав есть вероятность выхода за границы диапазона [0,1]
                noiseMap[y * width + x] = Mathf.Clamp01(noiseHeight);
            }
        }

        return GenerateWallsMap(width, height, TransformTo2DMap(noiseMap));
    }

    private float[,] TransformTo2DMap(float[] noiseMap)
    {
        int tbm = MapManager.tilesBeyoundMap;
        float[,] map = new float[height + 2 * tbm, width];
        for (int i = 0; i < height + 2*tbm; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (i < tbm || i >= height + tbm)
                {
                    map[i, j] = -1.0f;
                }
                else
                {
                    int x = j;
                    int y = i - tbm;
                    map[i, j] = noiseMap[y * width + x];
                }
            }
        }
        return map;
    }

    private int[,] GenerateWallsMap(int width, int height, float[,] noiseMap)
    {
        int tbm = MapManager.tilesBeyoundMap;
        int[,] tileMap = new int[height + 2 * tbm, width];
        for (int i = 0; i < height + 2 * tbm; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (noiseMap[i, j] == -1.0f)
                {
                    tileMap[i, j] = MapManager.MAP_BORDER;
                }
                else if (noiseMap[i, j] >= level && noiseMap[i, j] < level2)
                {
                    tileMap[i, j] = MapManager.WALL;
                }
                else if (noiseMap[i, j] >= level2)
                {
                    tileMap[i, j] = MapManager.MAP_BORDER;
                }
                else
                {
                    tileMap[i, j] = MapManager.FLOOR;
                }
            }
        }
        return tileMap;
    }
}
