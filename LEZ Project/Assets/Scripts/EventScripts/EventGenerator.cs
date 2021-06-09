using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventGenerator : MonoBehaviour
{
    public GameManager gameManager;
    public RoundResult roundResult;

    public GameObject eventBox;
    public Transform leftBox;
    public Transform rightBox;

    public Text titelText;
    public Text roleText;
    public GameObject actionPointManager;

    GameObject eventObject;
    List<GameObject> eventObjects = new List<GameObject>();

    public List<GameObject> sliderObjects = new List<GameObject>();
    public List<GameObject> toggleObjects = new List<GameObject>();
    public List<GameObject> actionObjects = new List<GameObject>();

    public GameObject sliderPrefab;
    public GameObject togglePrefab;
    public GameObject buttonPrefab;
    public GameObject actionCounterPrefab;
    public GameObject managerPointPrefab;

    void Start()
    {
        roundResult = this.GetComponent<RoundResult>();
    }

    /// <summary>
    /// With the StartEvent function every event is executed that is predefined with a CSV file 
    /// </summary>
    /// <param name="eventData"></param>
    public void StartEvent(TextAsset eventData)
    {
        //First the prepared event box is activated
        eventBox.SetActive(true);
        gameManager.isEventActiv = true;

        //The CSV file is then divided into individual lines 
        string[] data = eventData.text.Split(new char[] { '|' });

        //The individual CSV data should later be set from left to right 
        Transform startBox = leftBox;

        for (int i = 1; i < data.Length - 1; i++)
        {
            //The rows of the CSV file are divided into their individual cells so that each individual file can be read out 
            string[] row = data[i].Split(new char[] { ',' });

            //id/order row[0]
            //type row[1]
            //content row[2]
            //connected Variables row[3]
            //groups row[4]
            //connected Types[5]

            //If there is a line with zeros in the CSV file, this tells the event that the data should now be lined up in the right box 
            if (row[1] == "0")
            {
                startBox = rightBox;
            }
            //Now it goes through the individual rows of the CSV file, it is first checked which data type it is,
            //because all different data types must be processed differently in order to be displayed correctly in the event
            else
            {
                if (row[1] == "text")
                {
                    //The first information in a CSV file must always be the title of the event 
                    if (int.Parse(row[0]) == 0)
                    {
                        roleText.text = row[2];
                    }
                    else if (int.Parse(row[0]) == 1)
                    {
                        titelText.text = row[2];
                    }
                    else
                    {
                        //A new GameObject is created in which you insert a text component, to which you assign the data from the CSV. 
                        eventObject = new GameObject();
                        eventObject.transform.parent = startBox;

                        eventObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                        Text text = eventObject.AddComponent(typeof(Text)) as Text;

                        //The new text component needs some information to display the text correctly 
                        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                        text.fontSize = 40;
                        text.color = Color.black;

                        text.text = row[2];
                    }
                }
                else if (row[1] == "img")
                {
                    //A new GameObject is created in which you insert a image component.
                    eventObject = new GameObject();
                    eventObject.transform.parent = startBox;

                    //The data from the CSV file contain the name of the corresponding image, which is then searched for in the game's image folder 
                    Sprite eSprite = Resources.Load<Sprite>(row[2]);

                    eventObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                    eventObject.GetComponent<AspectRatioFitter>().aspectRatio = eSprite.rect.width / eSprite.rect.height;

                    eventObject.AddComponent<Image>().sprite = eSprite;
                }
                else if (row[1] == "slider")
                {
                    //A prepared prefab is taken from the game's assets, which then positions a slider in the event 
                    eventObject = Instantiate(sliderPrefab, startBox);

                    Slider eSlider = eventObject.GetComponentInChildren<Slider>();
                    EventSlider eventSlider = eSlider.GetComponent<EventSlider>();

                    //Because the content section of the Slider type in the CSV file must contain several files, they are now separated from one another 
                    string[] cell = row[2].Split(new char[] { '.' });

                    eventSlider.titleText.text = cell[0];      //The first is the text that titles the slider 
                    eSlider.value = int.Parse(cell[1]);                         //The second is the start value of the slider 
                    eSlider.minValue = int.Parse(cell[2]);                      //The third is the minimum value of the slider 
                    eSlider.maxValue = int.Parse(cell[3]);                      //The fourth is the maximum value of the slider 

                    eventSlider.gameManager = gameManager;

                    if (row[3] != "0")
                    {
                        //Because the slider has to access the basic values of the game, the CSV file specifies which of them the slider affects
                        string[] conVar = row[3].Split(new char[] { '.' });

                        //The fifth value of the contents indicates how the value of the slider is offset. 
                        //If it should be taken over 1:1, then the value of the slider is taken * 1. 
                        for (int j = 0; j < conVar.Length; j++)
                        {
                            //The EventSlider is then given the names of the basic values of the game and the values to be calculated. 
                            eventSlider.sliderValues.Add(conVar[j] + ":" + cell[j + 4]);
                        }
                    }

                    //Sliders can be grouped so that they react to the other sliders as soon as one is moved. 
                    eventSlider.sliderGroup = int.Parse(row[4]);

                    eventSlider.eventGenerator = this;

                    sliderObjects.Add(eventObject.GetComponentInChildren<Slider>().gameObject);

                    //Individual sections of the event can be assigned to toggles so that they can be deactivated or activated. 
                    if (row[5] != "0")
                    {
                        eventSlider.conToggle = row[5];
                    }
                }
                else if (row[1] == "toggle")
                {
                    //A prepared prefab is taken from the game's assets, which then positions a toggle in the event 
                    eventObject = Instantiate(togglePrefab, startBox);

                    Toggle eToggle = eventObject.GetComponent<Toggle>();

                    //Because a toggle can change several values of the game, they are written to the CSV file and then split up so that they can then be processed 
                    string[] cellContent = row[2].Split(new char[] { '.' });

                    //A toggle has a title / description which you have to include in the CSV file 
                    eToggle.GetComponentInChildren<Text>().text = cellContent[0];
                    //The CSV file also specifies whether the toggle is activated or deactivated at the beginning
                    eToggle.isOn = bool.Parse(cellContent[1]);

                    eventObject.GetComponent<EventToggle>().gameManager = gameManager;
                    eventObject.GetComponent<EventToggle>().eventGenerator = this;

                    if (row[3] != "0")
                    {
                        string[] cellConVar = row[3].Split(new char[] { '.' });

                        for (int j = 0; j < cellConVar.Length; j++)
                        {
                            //The content and the associated basic variable is passed to EventToggle 
                            eventObject.GetComponent<EventToggle>().toggleValues.Add(cellConVar[j] + ":" + cellContent[j + 2]);
                        }
                    }

                    //Toggles can be grouped so that they react to the other sliders as soon as one is moved. 
                    if (row[4] != "0")
                    {
                        eventObject.GetComponent<EventToggle>().toggleGroup = int.Parse(row[4]);
                    }

                    //Individual sections of the event can be assigned to toggles so that they can be deactivated or activated. 
                    if (row[5] != "0")
                    {
                        eventObject.GetComponent<EventToggle>().conToggle = row[5];
                    }

                    toggleObjects.Add(eventObject);
                }
                else if (row[1] == "button")
                {
                    //A prepared prefab is taken from the game's assets, which then positions a button in the event 
                    eventObject = Instantiate(buttonPrefab, startBox);

                    Button eButton = eventObject.GetComponent<Button>();

                    //Because a button can change several values of the game, they are written to the CSV file and then split up so that they can then be processed 
                    string[] cellContent = row[2].Split(new char[] { '.' });
                    string[] cellConVar = row[3].Split(new char[] { '.' });

                    //A button has a title / description which you have to include in the CSV file 
                    eButton.GetComponentInChildren<Text>().text = cellContent[0];
                    eventObject.GetComponent<EventButton>().buttonTitel = cellContent[0];

                    eventObject.GetComponent<EventButton>().gameManager = gameManager;
                    eventObject.GetComponent<EventButton>().eventGenerator = this;

                    //The button is given the values which then change based on the player's decision.
                    if (cellContent.Length > 1)
                    {
                        for (int j = 0; j < cellConVar.Length; j++)
                        {
                            string var = cellConVar[j];
                            string con = cellContent[j + 1];
                            eButton.onClick.AddListener(delegate { eventObject.GetComponent<EventButton>().SelectButton(var + ":" + con); });
                        }
                    }

                    eButton.onClick.AddListener(EndEvent);
                }
                else if (row[1] == "action")
                {
                    eventObject = Instantiate(actionCounterPrefab, startBox);

                    string[] cell = row[2].Split(new char[] { '.' });
                    string[] cellConVar = row[3].Split(new char[] { '.' });

                    ActionCounter actionCounter = eventObject.GetComponentInChildren<ActionCounter>();
                    actionCounter.actionManager = actionPointManager.GetComponent<ActionPointManager>();
                    actionCounter.gameManager = gameManager;

                    actionCounter.actionText.text = cell[0];
                    actionCounter.maxAction = cell.Length - 1;

                    for (int j = 1; j < cell.Length; j++)
                    {
                        actionCounter.actionContent.Add(cell[j]);
                    }
                    for (int j = 0; j < cellConVar.Length; j++)
                    {
                        actionCounter.conActionContent.Add(cellConVar[j]);
                    }

                    actionObjects.Add(eventObject);
                }
                else if (row[1] == "points")
                {
                    actionPointManager.SetActive(true);

                    for (int j = 0; j < int.Parse(row[2]); j++)
                    {
                        GameObject managerPoint = Instantiate(managerPointPrefab, actionPointManager.transform);
                        managerPoint.GetComponent<Toggle>().isOn = true;

                        actionPointManager.GetComponent<ActionPointManager>().managerPoints.Add(managerPoint);
                    }

                    actionPointManager.GetComponent<ActionPointManager>().pointsCount = int.Parse(row[2]);
                }

                //The entire content of the event is entered in a list 
                eventObjects.Add(eventObject);
            }
        }

        //The individual objects with different data types are now correctly assigned 
        for (int i = 0; i < toggleObjects.Count; i++)
        {
            toggleObjects[i].GetComponent<EventToggle>().ConnectObjects();
        }
    }

    /// <summary>
    /// Ends the event and thus deletes the entire content of the event 
    /// </summary>
    public void EndEvent()
    {
        eventBox.SetActive(false);
        gameManager.isEventActiv = false;

        foreach (GameObject item in sliderObjects)
        {
            if (item.activeSelf)
            {
                item.GetComponent<EventSlider>().SliderCalculator();
            }
        }

        foreach (GameObject item in toggleObjects)
        {
            item.GetComponent<EventToggle>().UseToggle();
        }

        foreach (GameObject item in actionObjects)
        {
            if (item.GetComponentInChildren<ActionCounter>().actionCounter != 0)
            {
                item.GetComponentInChildren<ActionCounter>().ActionResult();
            }
        }

        foreach (GameObject item in eventObjects)
        {
            Destroy(item);
        }

        foreach (GameObject item in actionPointManager.GetComponent<ActionPointManager>().managerPoints)
        {
            Destroy(item);
        }

        actionPointManager.SetActive(false);
        actionPointManager.GetComponent<ActionPointManager>().managerPoints.Clear();
        toggleObjects.Clear();
        sliderObjects.Clear();
        actionObjects.Clear();
        eventObjects.Clear();
    }
}
