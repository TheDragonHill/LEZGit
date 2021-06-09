using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The main task of the GameManager is to take care of the rounds of the scenario.
/// </summary>
public class GameManager : MonoBehaviour
{
	static public int round;
	public float roundTimer;

	public List<string> roundEvents = new List<string>();
	public List<GameObject> variableObjects = new List<GameObject>();
	public List<GameObject> roleLabels = new List<GameObject>();

	int currentEvent = 0;
	float eventInterval = 0;    //Indicates the time interval at which the events are displayed 

	public EventGenerator eventGenerator;

	public Transform hud;
	public GameObject basicVariablePrefab;

	public Slider roundDisplay;
	public Text roundNumberText;

	public bool isEventActiv = false;

	private void Awake()
	{
		//The scenario must first be loaded and the correct lap must be displayed at the beginning of each lap. 
		roundNumberText.text = (round + 1).ToString();

		LoadScenario();
	}

	private void Update()
	{
		if (!isEventActiv)
		{
			//In the round, the right events at the right time are always displayed with a timer. 
			if (roundTimer >= 60)
			{
				round++;
				roundTimer = 0;

				foreach (GameObject item in variableObjects)
				{
					string basicName = item.GetComponent<BasicVariable>().role;
					float basicNum = item.GetComponent<BasicVariable>().variableNumber;

					eventGenerator.gameObject.GetComponentInChildren<FeedbackManager>().StartFeedback(basicName, basicNum);
				}
				isEventActiv = true;
			}
			else if (roundTimer >= eventInterval)
			{
				eventInterval += 60 / (roundEvents.Count);

				eventGenerator.StartEvent(Resources.Load<TextAsset>(roundEvents[currentEvent]));
				currentEvent++;
			}
			else
			{
				roundTimer += Time.deltaTime;
				roundDisplay.value = roundTimer;
			}
		}
	}

	/// <summary>
	/// Loads all the important data for the scenario that were previously set in the menu. 
	/// </summary>
	void LoadScenario()
	{
		//Creates the different basic values that will (probably only) exist in this scenario. 
		for (int i = 0; i < ConstantVars.basicVariables.Count; i++)
		{
			GameObject variableObject = Instantiate(basicVariablePrefab, hud);

			BasicVariable basicVariable = variableObject.GetComponent<BasicVariable>();

			basicVariable.variableName = ConstantVars.basicVarName[i];
			basicVariable.variableNumber = ConstantVars.basicVariables[i];
			basicVariable.role = ConstantVars.roles[i];

			variableObjects.Add(variableObject);
		}

		//Loads the events of the respective round 
		string[] eventData = ConstantVars.scenarioEvents[round].Split(new char[] { '.' });

		for (int i = 0; i < eventData.Length; i++)
		{
			roundEvents.Add(eventData[i]);
		}
	}

	/// <summary>
	/// Allows you to change the basic values within the event. 
	/// </summary>
	/// <param name="varName"></param>
	/// <param name="econParam"></param>
	/// <param name="varValue"></param>
	public void LoadConVars(string varName, string econParam, string varValue)
	{
		for (int i = 0; i < variableObjects.Count; i++)
		{
			BasicVariable basicVariable = variableObjects[i].GetComponent<BasicVariable>();

			//You can specify in the CSV file how the basic values are changed 
			if (econParam == "direct")
			{
				basicVariable.variableNumber += int.Parse(varValue);
			}
			else if (econParam == "perTime")
			{
				basicVariable.perTimeValues.Add(int.Parse(varValue));
			}
			else if (econParam == "interval")
			{
				basicVariable.intervalValues.Add(int.Parse(varValue));
			}
		}
	}

	/// <summary>
	/// Restarts a round 
	/// </summary>
	public void EndRound()
	{
		ConstantVars.basicVarName.Clear();
		ConstantVars.basicVariables.Clear();

		for (int i = 0; i < variableObjects.Count; i++)
		{
			BasicVariable variable = variableObjects[i].GetComponent<BasicVariable>();

			ConstantVars.basicVarName.Add(variable.variableName);
			ConstantVars.basicVariables.Add(variable.variableNumber);
		}

		Application.LoadLevel(Application.loadedLevel);
	}

	/// <summary>
	/// Lets the scenario end, so you end up in the menu again. 
	/// </summary>
	public void EndScenario()
	{
		ConstantVars.scenarioEvents.Clear();
		ConstantVars.basicVarName.Clear();
		ConstantVars.basicVariables.Clear();
		ConstantVars.roundCounter = 1;
		round = 0;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
}
