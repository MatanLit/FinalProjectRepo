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
}
