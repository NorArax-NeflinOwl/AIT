using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Button : MonoBehaviour
{
    private Text textbox;
    private InputField input;

    private void Start()
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(ChangeText);

        textbox = GameObject.FindGameObjectWithTag("TextBox").GetComponent<Text>();
        input = GameObject.FindGameObjectWithTag("InputField").GetComponent<InputField>();
        input.onValueChanged.AddListener(delegate { InputFieldSelected(); });
    }

    private void InputFieldSelected()
    {
        textbox.text = input.text;
    }

    private void ChangeText()
    {
        string text = input.text;
        if(textbox.text.Equals(text))
        {
            textbox.text = "Text changed";
        }
        else
        {
            textbox.text = text;
        }
    }
}
