using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int UI_LAYER = 5;
    public static int GlobalKillCount = 0;
    List<string> uniqueKilledPlayerIds = new List<string>();
    List<string> playerIds = new List<string>();

    // List of spwan points in map. TODO: Some data structure that combines
    // Vector3 and boolean
    public static int[][] SpawnPoints = new int[10][]
    {
        new int[4] { 7, 0, 8, 0 },
        new int[4] { 20, 4, 26, 0 },
        new int[4] { 30, 0, 20, 0 },
        new int[4] { 40, 0, 20, 0 },
        new int[4] { 50, 0, 20, 0 },
        new int[4] { 60, 0, 20, 0 },
        new int[4] { 70, 0, 20, 0 },
        new int[4] { 80, 0, 20, 0 },
        new int[4] { 90, 0, 20, 0 },
        new int[4] { 100, 0, 20, 0 },
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
        GameObject[] timeShiftObjects = GameObject.FindGameObjectsWithTag("TimeShiftable");

        foreach (GameObject timeShiftObject in timeShiftObjects)
        {
            if (timeShiftObject.layer == UI_LAYER)
            {
                timeShiftObject.GetComponent<TimeShiftableObject>().TimeShift();
            }
        }
    }

    public static void ShiftTimeForAllUpgradeableObjects()
    {
        GameObject[] timeShiftObjects = GameObject.FindGameObjectsWithTag("TimeShiftable");

        foreach (GameObject timeShiftObject in timeShiftObjects)
        {
            timeShiftObject.GetComponent<TimeShiftableObject>().TimeShift();
        }
    }

    public static Vector3 GetAvailableSpawnPoint()
    {
        foreach (var spawnPoint in SpawnPoints)
        {
            if (spawnPoint[3] != 0)
            {
                // != 0 aka =1, which means the spawn point is taked
                continue;
            }
            
            Vector3 availablePosition = new Vector3(spawnPoint[0], spawnPoint[1], spawnPoint[2]);
            spawnPoint[3] = 1; // 1 = taken
            return availablePosition;
        }

        return new Vector3(0, 0, 0);
    }
}
