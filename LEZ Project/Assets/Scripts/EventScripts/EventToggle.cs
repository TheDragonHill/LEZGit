using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventToggle : MonoBehaviour
{
    public GameManager gameManager;
    public EventGenerator eventGenerator;

    public int toggleGroup;

    public List<string> toggleValues= new List<string>();

    public string conToggle = "0";
    public List<GameObject> conObjects = new List<GameObject>();

    public void ConnectObjects()
    {
        if (conToggle != "0")
        {
            for (int i = 0; i < eventGenerator.sliderObjects.Count; i++)
            {
                EventSlider eventSlider = eventGenerator.sliderObjects[i].GetComponent<EventSlider>();

                if (eventSlider.conToggle != "0")
                {
                    string[] tConData = conToggle.Split(new char[] { '.' });
                    string[] sConData = eventSlider.conToggle.Split(new char[] { '.' });

                    if (tConData[0] == sConData[0] && tConData[1] != sConData[1])
                    {
                        eventGenerator.sliderObjects[i].transform.parent.gameObject.SetActive(false);
                        this.GetComponent<Toggle>().onValueChanged.AddListener(eventGenerator.sliderObjects[i].transform.parent.gameObject.SetActive);
                    }
                }
            }
        }
    }

    public void UseToggle()
    {
        if (GetComponent<Toggle>().isOn)
        {
            for (int i = 0; i < toggleValues.Count; i++)
            {
                string[] value = toggleValues[i].Split(new char[] { ':' });

                gameManager.LoadConVars(value[0], value[1], value[2]);
            }
        }
    }

    public void ConnectedToggle()
    {
        if (toggleGroup != 0 && GetComponent<Toggle>().isOn)
        {
            foreach (GameObject item in eventGenerator.toggleObjects)
            {
                if (item != this.gameObject && item.GetComponent<EventToggle>().toggleGroup == toggleGroup)
                {
                    item.GetComponent<Toggle>().isOn = false;
                }
            }
        }

        if (conToggle != null)
        {
            for (int i = 0; i < conObjects.Count; i++)
            {
                conObjects[i].SetActive(false);
            }
        }
    }
}
