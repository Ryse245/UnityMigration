using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderText : MonoBehaviour
{
    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        Slider parent = GetComponentInParent<Slider>();
        Text text = GetComponent<Text>();

        text.text = parent.value + "x";
    }
}
