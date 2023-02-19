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
    public static int[][] SpawnPoints = new int[30][]
    {
        new int[4] { 7, 0, 8, 0 },
        new int[4] { 20, 4, 25, 0 },
        new int[4] { 20, 0, -4, 0 },
        new int[4] { 25, 5, 6, 0 },
        new int[4] { 52, 0, 10, 0 },
        new int[4] { 40, 0, 45, 0 },
        new int[4] { 40, 0, 73, 0 },
        new int[4] { 17, 0, 42, 0 },
        new int[4] { 10, 10, 45, 0 },
        new int[4] { 20, 0, 72, 0 },
        new int[4] { 7, 0, 8, 0 },
        new int[4] { 20, 4, 25, 0 },
        new int[4] { 6, 0, 60, 0 },
        new int[4] { -20, 0, 68, 0 },
        new int[4] { -5, 8, 21, 0 },
        new int[4] { -37, 0, 13, 0 },
        new int[4] { -50, 0, 46, 0 },
        new int[4] { -38, 0, 46, 0 },
        new int[4] { -45, 0, 65, 0 },
        new int[4] { -33, 0, 64, 0 },
        new int[4] { 53, 0, 14, 0 },
        new int[4] { 29, 0, -28, 0 },
        new int[4] { 32, 0, 14, 0 },
        new int[4] { 17, 5, 10, 0 },
        new int[4] { 31, 0, 39, 0 },
        new int[4] { 46, 0, 39, 0 },
        new int[4] { 47, 0, 10, 0 },
        new int[4] { 47, 6, 10, 0 },
        new int[4] { 53, 6, -14, 0 },
        new int[4] { 39, 0, 39, 0 },
    };

    void Start() { }

    void Update()
    {
        // print($"GlobalKillCount: {GlobalKillCount}");
        if (GlobalKillCount >= 1)
        {
            GlobalKillCount = 0;
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
        // TODO: Hold index of last spawn point used instead of looping through all
        foreach (int[] spawnPoint in SpawnPoints)
        {
            if (spawnPoint[3] != 0)
            {
                // != 0 aka =1, which means the spawn point is taken
                continue;
            }
            
            Vector3 availablePosition = new Vector3(spawnPoint[0], spawnPoint[1], spawnPoint[2]);
            spawnPoint[3] = 1; // 1 = taken
            print($"SpawnPoint: {availablePosition}");
            return availablePosition;
        }

        return new Vector3(0, 10, 0);
    }
}
