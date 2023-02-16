using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeShiftableObject : MonoBehaviour
{
    [SerializeField] int timeShiftLevel = 0;
    [SerializeField] List<GameObject> upgradeables;
    
    void Start()
    {
        UpdateMesh();
    }

    public void TimeShift()
    {
        if (timeShiftLevel + 1 >= upgradeables.Count)
        {
            timeShiftLevel = 0;
        } else
        {
            timeShiftLevel++;
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        GetComponent<MeshFilter>().sharedMesh = upgradeables[timeShiftLevel].GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshRenderer>().sharedMaterial = upgradeables[timeShiftLevel].GetComponent<MeshRenderer>().sharedMaterial;
    }
}
