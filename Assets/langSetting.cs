using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class langSetting : MonoBehaviour {

    [Multiline]
    public string ITA_Text;
    private Text myText;
    private TMP_Text TMmyText;

    void Awake()
    {
        myText = GetComponent<Text>();
        TMmyText = GetComponent<TMP_Text>();
    }


    void Start () {
        SetLang();
    }


    void SetLang()
    {

        if (Application.systemLanguage.ToString() == "Italian")
        {
            if (myText&&ITA_Text != "")
                myText.text = ITA_Text;

            if (TMmyText && ITA_Text != "")
                TMmyText.text = ITA_Text;
        }
    }

}
