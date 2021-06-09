using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject scenarioBox;
    bool isScenario = false;

    public Transform scenarioPos;
    public GameObject scenarioPrefab;

    public Text scenarioTitleText;
    public Text scenarioDescText;

    public Button scenConButton;

    public Text roundText;

    private void Update()
    {
        //Starts and configures the complex scenario menu.
        if (scenarioBox.activeSelf && !isScenario)
        {
            isScenario = true;

            ConfigureScenario();
        }

        roundText.text = ConstantVars.roundCounter.ToString();
    }

    /// <summary>
    /// All data will be read from the CSV file and loaded into the various scenarios.
    /// </summary>
    void ConfigureScenario()
    {
        TextAsset scenarioSelection = Resources.Load<TextAsset>("ScenarioSelection");
        string[] data = scenarioSelection.text.Split(new char[] { '|' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            //id row[0]
            //title row[1]
            //descibtiom row[2]
            //basic variables row[3]
            //round0-4 row[4-8]

            string title = row[1];
            string desc = row[2];

            //For each scenario, a button is created in the scenario menu that can pass on all the data relating to its scenario.  
            GameObject scenarioObject = Instantiate(scenarioPrefab, scenarioPos);

            scenarioObject.GetComponentInChildren<Text>().text = title;

            //This data will be attached to the buttons 
            Button scenarioButton = scenarioObject.GetComponent<Button>();

            scenarioButton.onClick.AddListener(delegate { SetTitleText(title); });
            scenarioButton.onClick.AddListener(delegate { SetDescText(desc); });
            scenarioButton.onClick.AddListener(BackToScenario);

            scenarioButton.onClick.AddListener(delegate { scenConButton.gameObject.SetActive(true); });
            scenarioButton.onClick.AddListener(delegate { scenConButton.onClick.AddListener(delegate { SetBasicVariables(row[3]); }); });
            scenarioButton.onClick.AddListener(delegate { scenConButton.onClick.AddListener(delegate { SetScenarios(row[4] + "," + row[5] + "," + row[6] + "," + row[7] + "," + row[8]); }); });
        }
    }

    /// <summary>
    /// Submits the title of the scenario 
    /// </summary>
    /// <param name="content"></param>
    public void SetTitleText(string content)
    {
        scenarioTitleText.text = content;
    }

    /// <summary>
    /// Submits the description of the scenario 
    /// </summary>
    /// <param name="content"></param>
    public void SetDescText(string content)
    {
        scenarioDescText.text = content;
    }

    /// <summary>
    /// Transmits the events of the scenario 
    /// </summary>
    /// <param name="scenarios"></param>
    public void SetScenarios(string scenarios)
    {
        string[] scenSplit = scenarios.Split(new char[] { ',' });

        for (int i = 0; i < scenSplit.Length; i++)
        {
            ConstantVars.scenarioEvents.Add(scenSplit[i]);
        }
    }

    /// <summary>
    /// Conveys the basic values of the scenario 
    /// </summary>
    /// <param name="variable"></param>
    public void SetBasicVariables(string variable)
    {
        string[] cell = variable.Split(new char[] { '.' });

        for (int i = 0; i < cell.Length; i++)
        {
            string[] variablesData = cell[i].Split(new char[] { ':' });

            ConstantVars.basicVarName.Add(variablesData[0]);
            ConstantVars.basicVariables.Add(float.Parse(variablesData[1]));
            ConstantVars.roles.Add(variablesData[2]);
        }
    }

    /// <summary>
    /// Sets the number of rounds 
    /// </summary>
    /// <param name="plus"></param>
    public void SetRoundCounter(bool plus)
    {
        if (plus && ConstantVars.roundCounter < 5)
        {
            ConstantVars.roundCounter++;
        }
        else if (!plus && ConstantVars.roundCounter > 1)
        {
            ConstantVars.roundCounter--;
        }
    }

    public void BackToScenario()
    {
        ConstantVars.scenarioEvents.Clear();
        ConstantVars.basicVariables.Clear();
        scenConButton.onClick.RemoveAllListeners();
    }

    public void StartScenario()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [System.Obsolete]
    public void ResetMenu()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
