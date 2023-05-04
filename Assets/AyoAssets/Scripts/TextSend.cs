using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextSend : MonoBehaviour
{
    public TMP_InputField  barField;
    void Awake()
    {
        //barField = null;
    }
    // Start is called before the first frame update
    void Update()
    {
        updateBar();
    }

    // Update is called once per frame
    public void updateBar()
    {
        ManagerVar.Instance.barValue = barField.text;
    }
}
