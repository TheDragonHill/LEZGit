using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves and scales the title of the event box exactly in the right place. 
/// </summary>
public class Titel : MonoBehaviour
{
    public RectTransform rectEventBox;
    public RectTransform rectTransform;
    public RectTransform titelTextField;

    public float sidePadding = 40f;
    public float space = 560f;

    void Update()
    {
        rectTransform.sizeDelta = new Vector2(titelTextField.sizeDelta.x + sidePadding, 60f);
        transform.position = new Vector3(space + (rectTransform.rect.width / 1.99536f), transform.position.y);
        titelTextField.gameObject.transform.position = transform.position;
    }
}
