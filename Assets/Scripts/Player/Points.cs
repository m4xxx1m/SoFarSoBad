using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points
{
    private static List<Points> instanceList;
    public static List<Points> getInstanceList()
    {
        return instanceList;
    }
    public static Points getCurrentInstance()
    {
        return instanceList[instanceList.Count - 1];
    }
    public static int CurrentChunk = 1;

    public const int pointsForGear = 10;
    public const int pointsForVruden = 10;
    public const int pointsForGrohog = -10;
    public const int pointsForTroned = 500;
    public const int pointsFromChest = 50;

    public static PointCounter Counter;
    public int pointCounter = 0;

    public Points()
    {
        instanceList ??= new List<Points>();
        instanceList.Add(this);
    }
}
