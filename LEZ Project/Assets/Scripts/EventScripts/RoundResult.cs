using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundResult : MonoBehaviour
{
    GameManager gameManager;
    EventGenerator eventGenerator;

    GameObject resultObject;

    public List<string> eventDecisions = new List<string>();

    public GameObject graphPrefab;
    public GameObject ratingPrefab;

    private void Start()
    {
        eventGenerator = this.GetComponent<EventGenerator>();
        gameManager = eventGenerator.gameManager;
    }

    /// <summary>
    /// Starts the final result that is displayed after each round 
    /// </summary>
    public void StartResult()
    {
        eventGenerator.eventBox.SetActive(true);

        eventGenerator.titelText.text = "Ergebnis nach " + (GameManager.round) + " Jahren:";

        //Shows the decisions the player made this round 
        for (int i = 0; i < eventDecisions.Count; i++)
        {
            resultObject = new GameObject();
            resultObject.transform.parent = eventGenerator.leftBox;

            resultObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Text text = resultObject.AddComponent<Text>();

            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.fontSize = 30;
            text.color = Color.black;

            text.text = "Ereignis " + (i + 1) + ":\n" + "Du hast du dich für die " + eventDecisions[i] + " entschieden";
        }

        GameObject ratingBox = Instantiate(new GameObject(), eventGenerator.rightBox);
        ratingBox.AddComponent<HorizontalLayoutGroup>();

        for (int i = 0; i < gameManager.variableObjects.Count; i++)
        {
            SetRating(gameManager.variableObjects[i].GetComponent<BasicVariable>().variableNumber, gameManager.variableObjects[i].GetComponent<BasicVariable>().role, ratingBox.transform);
        }
        //SetGraph();

        Button endRoundButton = Instantiate(eventGenerator.buttonPrefab, eventGenerator.rightBox).GetComponent<Button>();
        Destroy(endRoundButton.GetComponent<EventButton>());
        endRoundButton.GetComponentInChildren<Text>().text = "Runde beenden";

        if (GameManager.round == ConstantVars.roundCounter)
        {
            endRoundButton.onClick.AddListener(gameManager.EndScenario);
        }
        else
        {
            endRoundButton.onClick.AddListener(gameManager.EndRound);
        }
    }

    void SetRating(float resultVar, string varName, Transform ratingBox)
    {
        TextAsset feedbackData = Resources.Load<TextAsset>("FeedbackData");
        string[] data = feedbackData.text.Split(new char[] { '|' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] == varName)
            {
                string[] cell = row[6].Split(new char[] { '-' });

                for (int x = 0; x < cell.Length; x++)
                {
                    if (x == cell.Length - 1)
                    {
                        return;
                    }
                    else if (resultVar >= float.Parse(cell[x]) && resultVar <= float.Parse(cell[x + 1]))
                    {
                        resultObject = Instantiate(ratingPrefab, ratingBox);

                        float perc = float.Parse(cell[x + 1]) - float.Parse(cell[x]);
                        resultVar -= float.Parse(cell[x]);

                        perc = ((resultVar * 100) / perc) / 10;
                        resultObject.GetComponentInChildren<Text>().text = (x.ToString()) + "," + (Mathf.Round(perc).ToString());
                        resultObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = varName; 
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a graph that shows how the basic values have changed during the round, which shows what the player's decisions have led to. 
    /// </summary>
    void SetGraph()
    {
        resultObject = Instantiate(graphPrefab, eventGenerator.rightBox);

        UIGridRenderer gridRenderer = resultObject.GetComponentInChildren<UIGridRenderer>();

        //There are different colors for the lines of the graph so that you can better distinguish them. 
        List<Color> lineColors = new List<Color>();
        lineColors.Add(Color.red);
        lineColors.Add(Color.blue);
        lineColors.Add(Color.green);
        lineColors.Add(Color.yellow);
        lineColors.Add(Color.magenta);
        lineColors.Add(Color.cyan);

        for (int i = 0; i < gameManager.variableObjects.Count; i++)
        {
            BasicVariable basicVariable = gameManager.variableObjects[i].GetComponent<BasicVariable>();
            UILineRenderer lineRenderer;

            GameObject lineDesc;

            if (i == 0)
            {
                lineRenderer = gridRenderer.lineRenderer.GetComponent<UILineRenderer>();

                lineDesc = gridRenderer.lineDesc;
            }
            else
            {
                lineRenderer = Instantiate(gridRenderer.lineRenderer.gameObject, gridRenderer.gameObject.transform.parent).GetComponent<UILineRenderer>();

                lineDesc = Instantiate(gridRenderer.lineDesc, gridRenderer.description);
            }

            int lineColorNumber = Random.Range(0, lineColors.Count);
            lineRenderer.color = lineColors[lineColorNumber];
            lineDesc.GetComponentInChildren<Image>().color = lineColors[lineColorNumber];
            lineColors.RemoveAt(lineColorNumber);

            lineDesc.GetComponentInChildren<Text>().text = basicVariable.variableName;

            //Sets the coordinates of the points 
            for (int j = 0; j < basicVariable.intermediateResult.Count; j++)
            {
                Vector2 point = lineRenderer.points[j];

                point.y = basicVariable.intermediateResult[j] / 100;

                lineRenderer.points[j] = point;
            }

            lineRenderer.points[lineRenderer.points.Count - 1] = new Vector2(12, basicVariable.variableNumber / 100);
        }
    }
}
