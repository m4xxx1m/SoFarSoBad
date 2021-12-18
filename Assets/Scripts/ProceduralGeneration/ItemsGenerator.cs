using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsGenerator
{
    private int width;
    private int height;
    private int[,] wallsMap;
    private int count;
    private int type;

    public ItemsGenerator(int width, int height, ref int[,] wallsMap, int count, int type)
    {
        this.width = width;
        this.height = height;
        this.wallsMap = wallsMap;
        this.count = count;
        this.type = type;
    }

    public int[,] GenerateItemMap(int[,] itemMap)
    {
        int tbm = MapManager.tilesBeyoundMap;
        for (int i = 0; i < count; ++i)
        {
            int x, y;
            do
            {
                y = Random.Range(0, height) + tbm;
                x = Random.Range(10, width);
            } while (wallsMap[y, x] != MapManager.FLOOR || itemMap[y, x] != 0);
            itemMap[y, x] = type;
        }
        return itemMap;
    }
}
