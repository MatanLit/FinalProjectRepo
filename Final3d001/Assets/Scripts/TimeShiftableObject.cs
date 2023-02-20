using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: GameManager should inherit from NetworkBehaviour and transfer data to all objects
public class TimeShiftableObject : MonoBehaviour
{
    [SerializeField]
    int timeShiftLevel = 0;

    [SerializeField]
    List<GameObject> upgradeables;

    void Start()
    {
        for (int i = 0; i < upgradeables.Count; i++)
        {
            Vector3 position = new Vector3(0, 0.2f, 0.6f);
            GameObject upgradeableObject = Instantiate(upgradeables[i], position, Quaternion.identity);
            upgradeableObject.transform.parent = transform;
            upgradeableObject.SetActive(i == timeShiftLevel);
        }
    }

    public void TimeShift()
    {
        if (timeShiftLevel + 1 >= upgradeables.Count)
        {
            timeShiftLevel = 0;
        }
        else
        {
            timeShiftLevel++;
        }

        for (int i = 0; i < upgradeables.Count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == timeShiftLevel);
        }
    }
}
