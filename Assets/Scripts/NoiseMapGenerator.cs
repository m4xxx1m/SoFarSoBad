using UnityEngine;

public static class NoiseMapGenerator
{
    public static float[] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
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
            float xOffset = rand.Next(-100000, 100000) + offset.x;
            float yOffset = rand.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(xOffset / width, yOffset / height);
        }

        if (scale < 0)
        {
            scale = 0.0001f;
        }

        // Учитываем половину ширины и высоты, для более визуально приятного изменения масштаба
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

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
                    float xResult = (x - halfWidth) / scale * frequency + octavesOffset[i].x * frequency;
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

        return noiseMap;
    }
}
