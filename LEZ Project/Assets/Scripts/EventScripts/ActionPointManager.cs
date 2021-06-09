using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointManager : MonoBehaviour
{
    public List<GameObject> managerPoints = new List<GameObject>();

    public int pointsCount;

    private void Update()
    {
        if (pointsCount <= 0)
        {
            pointsCount = 0;
        }
    }

    public void AddManagerPoint()
    {
        for (int i = 0; i < pointsCount; i++)
        {
            managerPoints[i].GetComponent<Toggle>().isOn = true;
        }
    }

    public void RemoveManagerPoint()
    {
        for (int i = managerPoints.Count; i > pointsCount; i--)
        {
            managerPoints[i - 1].GetComponent<Toggle>().isOn = false;
        }
    }
}
