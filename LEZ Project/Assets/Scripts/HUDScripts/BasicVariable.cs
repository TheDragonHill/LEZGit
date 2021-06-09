using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains all the important values individually for the various basic values. 
/// </summary>
public class BasicVariable : MonoBehaviour
{
	public Text variableNameText;

	public string variableName;
	public float variableNumber;
	public string role;

	public List<float> perTimeValues = new List<float>();
	public List<float> intervalValues = new List<float>();
	public List<float> intermediateResult = new List<float>();

	float variableTimer;

	GameManager gameManager;

	GameObject conRoleLabel;

	private void Start()
	{
		variableNameText.text = variableName;

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		intermediateResult.Add(variableNumber);

		foreach (GameObject item in gameManager.roleLabels)
		{
			if (item.tag == role)
			{
				conRoleLabel = item;
			}
		}
	}

	private void Update()
	{
		//Changes the basic values every 10 seconds (12 times in one round) based on the data that were transmitted to the script at the beginning 
		if (variableTimer >= 10)
		{
			foreach (float perTime in perTimeValues)
			{
				variableNumber -= perTime;
			}
			for (int i = 0; i < intervalValues.Count; i++)
			{
				float ranNum = intervalValues[i] / Random.Range(1, intervalValues[i] / 6);

				variableNumber -= ranNum;

				intervalValues[i] -= ranNum;
			}

			intermediateResult.Add(variableNumber);

			SetValueGraph(variableNumber);

			conRoleLabel.GetComponent<RoleLabels>().incomeAngle = variableNumber;

			variableTimer = 0;
		}
		else if (!gameManager.isEventActiv)
		{
			variableTimer += Time.deltaTime;
		}
	}

	void SetValueGraph(float yLineAxis)
	{
		UIGridRenderer gridRenderer = this.gameObject.GetComponentInChildren<UIGridRenderer>();
		UILineRenderer lineRenderer = gridRenderer.lineRenderer.GetComponent<UILineRenderer>();

		//Sets the coordinates of the points 
		Vector2 point = new Vector2(lineRenderer.points.Count, yLineAxis / 100);
		lineRenderer.points.Add(point);

		lineRenderer.enabled = false;
		lineRenderer.enabled = true;
	}
}
