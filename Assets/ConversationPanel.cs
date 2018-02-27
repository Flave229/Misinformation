using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour
{
    public GameObject _convoPanel;
    public GameObject _hideButton;
    public bool panelIsHidden = false;
	// Use this for initialization
	void Start () {
        //_convoPanel = GameObject.FindGameObjectWithTag("ConversationPanel").gameObject.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject;
        _convoPanel = GameObject.FindGameObjectWithTag("ConversationPanel");
        _hideButton = GameObject.FindGameObjectWithTag("ConversationPanel").gameObject.transform.parent.Find("ButtonHide").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HidePanel()
    {
        if (!panelIsHidden)
        {
            _convoPanel.SetActive(false);
            // _hideButton = GameObject.FindGameObjectWithTag("ConversationPanel").gameObject.transform.Find("ButtonHide").gameObject;
            _hideButton.gameObject.transform.Find("TextBtn").GetComponent<Text>().text = "▲";
            panelIsHidden = true;
        }
        else
        {
            _convoPanel.SetActive(true);
            // _hideButton = GameObject.FindGameObjectWithTag("ConversationPanel").gameObject.transform.Find("ButtonHide").gameObject;
            _hideButton.gameObject.transform.Find("TextBtn").GetComponent<Text>().text = "▼";
            panelIsHidden = false;
        }

    }
}
