using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeableObject
{
    int level = 0;
    public string name = "";
    List<GameObject> upgradeables;
    
    public UpgradeableObject(string name, List<GameObject> upgradeables)
    {
        this.name = name;
        this.upgradeables = upgradeables;
    }

    public void TimeShift()
    {
        if (level + 1 >= upgradeables.Count)
        {
            level = 0;
            return;
        }

        level++;
    }

    public GameObject GetTimeShiftedObject()
    {
        return upgradeables[level];
    }
}

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
    }

    void Update()
    {
        
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
