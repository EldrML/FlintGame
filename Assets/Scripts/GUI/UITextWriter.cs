using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UITextWriter : MonoBehaviour
{

	[TextArea]
    public string completeText;
    private Text textComponent;

    [Range(0, 100)]
    public int characterPercentageToShow = 100;

    // Use this for initialization
    void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string textToDisplay = completeText.Substring(0, Mathf.CeilToInt(characterPercentageToShow / 100f * completeText.Length));
        textComponent.text = textToDisplay;
    }
}
