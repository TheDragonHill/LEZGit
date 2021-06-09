using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public List<GameObject> feedbackBoxes = new List<GameObject>();

    public GameObject feedbackBox;

    public void StartFeedback(string basicVarName, float resultVariable)
    {
        feedbackBox.SetActive(true);

        string feedbackText = "";

        TextAsset feedbackData = Resources.Load<TextAsset>("FeedbackData");
        string[] data = feedbackData.text.Split(new char[] { '|' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            //id/order row[0]
            //role row[1]
            //opinionValues row[2]
            //bad row[3]
            //neutral row[4]
            //good Types[5]

            if (row[1] == basicVarName)
            {
                string[] cell = row[2].Split(new char[] { '-' });

                if (float.Parse(cell[0]) >= resultVariable)
                {
                    feedbackText = row[3];
                }
                else if (float.Parse(cell[0]) <= resultVariable && float.Parse(cell[1]) >= resultVariable)
                {
                    feedbackText = row[4];
                }
                else if (float.Parse(cell[1]) <= resultVariable)
                {
                    feedbackText = row[5];
                }
            }
        }

        foreach (GameObject item in feedbackBoxes)
        {
            if (item.tag == basicVarName)
            {
                item.SetActive(true);

                item.GetComponentInChildren<Text>().text = feedbackText;
            }
        }
    }
}
