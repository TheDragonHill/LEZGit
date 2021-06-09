using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCounter : MonoBehaviour
{
    public int actionCounter = 0;
    public Text actionText;
    public Text actionCount;

    public int maxAction;
    public int minAction = 0;

    public List<string> actionContent = new List<string>();
    public List<string> conActionContent = new List<string>();

    public ActionPointManager actionManager;
    public GameManager gameManager;

    /// <summary>
    /// Sets the number of rounds 
    /// </summary>
    /// <param name="plus"></param>
    public void SetRoundCounter(bool plus)
    {
        if (actionManager.pointsCount != 0)
        {
            if (plus && actionCounter < maxAction)
            {
                actionCounter++;
                actionManager.pointsCount--;
                actionManager.RemoveManagerPoint();
            }
            else if (!plus && actionCounter > minAction)
            {
                actionCounter--;
                actionManager.pointsCount++;
                actionManager.AddManagerPoint();
            }

            if (actionCounter > 0)
            {
                gameObject.GetComponent<Toggle>().isOn = true;
            }
            else
            {
                gameObject.GetComponent<Toggle>().isOn = false;
            }
        }
        else if (actionManager.pointsCount == 0 && actionCounter != 0 && !plus && actionCounter > minAction)
        {
            actionCounter--;
            actionManager.pointsCount++;
            actionManager.AddManagerPoint();
        }

        actionCount.text = actionCounter.ToString();
    }

    public void ActionResult()
    {
        string[] resultAction = actionContent[actionCounter - 1].Split(new char[] { ':' });

        for (int i = 0; i < resultAction.Length; i++)
        {
            string[] conContent = conActionContent[i].Split(new char[] { ':' });

            gameManager.LoadConVars(conContent[0], conContent[1], resultAction[i]);
        }
    }
}
