using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSlider : MonoBehaviour
{
    public EventGenerator eventGenerator;
    public GameManager gameManager;

    public Text titleText;
    public Text minValueText;
    public Text maxValueText;

    public int sliderGroup;

    public List<string> sliderValues = new List<string>();

    public string conToggle;

    private void Start()
    {
        minValueText.text = GetComponentInChildren<Slider>().minValue.ToString();
        maxValueText.text = GetComponentInChildren<Slider>().maxValue.ToString();
    }

    public void SliderCalculator()
    {
        for (int i = 0; i < sliderValues.Count; i++)
        {
            string[] valueSplit = sliderValues[i].Split(new char[] { ':' });
            //valueSplit[0] = conVar
            //valueSplit[1] = econParam
            //valueSplit[2] = operator
            //valueSplit[3] = term

            float result = 0;
            if (valueSplit[2] == "+")
            {
                result = GetComponentInChildren<Slider>().value + float.Parse(valueSplit[3]);
            }
            else if (valueSplit[2] == "-")
            {
                result = GetComponentInChildren<Slider>().value - float.Parse(valueSplit[3]);
            }
            else if (valueSplit[2] == "*")
            {
                result = GetComponentInChildren<Slider>().value * float.Parse(valueSplit[3]);
            }
            else if (valueSplit[2] == "/")
            {
                result = GetComponentInChildren<Slider>().value / float.Parse(valueSplit[3]);
            }

            result = Mathf.RoundToInt(result);

            gameManager.LoadConVars(valueSplit[0], valueSplit[1], result.ToString());
        }
    }

    public void ConnectedSlider()
    { 
        if (sliderGroup != 0)
        {
            foreach (GameObject item in eventGenerator.sliderObjects)
            {
                if (item != this.gameObject && item.GetComponent<EventSlider>().sliderGroup == sliderGroup)
                {
                    item.GetComponent<Slider>().value = item.GetComponent<Slider>().minValue;
                }
            }
        }
    }
}
