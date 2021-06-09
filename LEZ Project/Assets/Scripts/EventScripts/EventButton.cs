using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    public GameManager gameManager;
    public EventGenerator eventGenerator;

    public string buttonTitel;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(TransferTitel);
    }

    public void SelectButton(string contentVar)
    {
        string[] worth = contentVar.Split(new char[] { ':' });
        //worth[0] connectedVar
        //worth[1] econParam
        //worth[2] content

        gameManager.LoadConVars(worth[0], worth[1], worth[2]);
    }

    public void TransferTitel()
    {
        eventGenerator.roundResult.eventDecisions.Add(buttonTitel);
    }
}
