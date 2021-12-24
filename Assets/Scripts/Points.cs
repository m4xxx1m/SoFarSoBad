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
    public const int pointsForGrohog = 6;
    public const int pointsForTroned = 500;

    // [SerializeField] private PointCounter pointCounter;
    public int pointCounter = 0;

    public Points()
    {
        if (instanceList == null)
        {
            instanceList = new List<Points>();
        }
        instanceList.Add(this);
    }
}
