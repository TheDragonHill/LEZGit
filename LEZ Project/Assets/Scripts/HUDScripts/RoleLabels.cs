using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleLabels : MonoBehaviour
{
    public float incomeAngle = 0;

    public RectTransform arrowTransform;

    private void Update()
    {
        if (incomeAngle == 0)
        {
            arrowTransform.rotation = Quaternion.EulerAngles(0, 0, 0);
        }
        else if (0 < incomeAngle)
        {
            arrowTransform.rotation = Quaternion.EulerAngles(0, 0, 90f);
        }
        else if (0 > incomeAngle)
        {
            arrowTransform.rotation = Quaternion.EulerAngles(0, 0, -90f);
        }
    }
}
