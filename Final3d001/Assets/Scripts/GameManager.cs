using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class UpgradeableObject
//{
//    int level = 0;
//    public string name = "";
//    List<GameObject> upgradeables;

//    public UpgradeableObject(string name, List<GameObject> upgradeables)
//    {
//        this.name = name;
//        this.upgradeables = upgradeables;
//    }

//    public void TimeShift()
//    {
//        if (level + 1 >= upgradeables.Count)
//        {
//            level = 0;
//            return;
//        }

//        level++;
//    }

//    public GameObject GetTimeShiftedObject()
//    {
//        return upgradeables[level];
//    }
//}

public class GameManager : MonoBehaviour
{
    static int UI_LAYER = 5;
    public static int globalKillCount = 0;
    List<string> uniqueKilledPlayerIds = new List<string>();
    List<string> playerIds = new List<string>();

    // List of spwan points in map. TODO: Some data structure that combines
    // Vector3 and boolean
    public static int[][] spawnPoints = new int[10][]
    {
        new int[3] { 7, 0, 8 },
        new int[3] { 20, 4, 26 },
        new int[3] { 30, 0, 20 },
        new int[3] { 40, 0, 20 },
        new int[3] { 50, 0, 20 },
        new int[3] { 60, 0, 20 },
        new int[3] { 70, 0, 20 },
        new int[3] { 80, 0, 20 },
        new int[3] { 90, 0, 20 },
        new int[3] { 100, 0, 20 },
    };

    void Start() { }

    void Update()
    {
        if (uniqueKilledPlayerIds.Count >= playerIds.Count / 2)
        {
            ShiftTimeForAllUpgradeableUIObjects();
        }
    }

    public static void ShiftTimeForAllUpgradeableUIObjects()
    {
        GameObject[] timeShiftablesObjects = GameObject.FindGameObjectsWithTag("TimeShiftable");

        foreach (GameObject timeshiftableObject in timeShiftablesObjects)
        {
            if (timeshiftableObject.layer == UI_LAYER)
            {
                timeshiftableObject.GetComponent<TimeShiftableObject>().TimeShift();
            }
        }
    }

    public static void ShiftTimeForAllUpgradeableObjects()
    {
        GameObject[] timeShiftablesObjects = GameObject.FindGameObjectsWithTag("TimeShiftable");

        foreach (GameObject timeshiftableObject in timeShiftablesObjects)
        {
            timeshiftableObject.GetComponent<TimeShiftableObject>().TimeShift();
        }
    }

    public static Vector3 GetAvailableSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i][2] == 1)
            {
                // FIXME:
                return new Vector3(spawnPoints[i][0], spawnPoints[i][1], spawnPoints[i][2]);
            }
        }

        return new Vector3(0, 0, 0);
    }
}
