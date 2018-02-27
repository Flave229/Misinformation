using Assets.Scripts.EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour, IEventListener
{
    public GameObject _convoPanel;
    public GameObject _hideButton;
    public bool panelIsHidden;
	// Use this for initialization
	void Start()
    {
        SubscribeToEvents();
        _convoPanel = GameObject.FindGameObjectWithTag("ConversationPanel");
        _hideButton = GameObject.FindGameObjectWithTag("ConversationPanel").gameObject.transform.parent.Find("ButtonHide").gameObject;
    }
	


	// Update is called once per frame
	void Update ()
    {
	}

    public void ShowPanel()
    {
        _hideButton.gameObject.transform.Find("TextBtn").GetComponent<Text>().text = "▼";
        PanelToggle(false);
    }

    public void HidePanel()
    {
        _hideButton.gameObject.transform.Find("TextBtn").GetComponent<Text>().text = "▲";
        PanelToggle(true);
    }

    private void PanelToggle(bool hidePanel)
    {
        panelIsHidden = hidePanel;
        _convoPanel.SetActive(!panelIsHidden);
    }

    public void ConsumeEvent(Assets.Scripts.EventSystem.Event subscribeEvent, object eventPacket)
    {

    }

    public void SubscribeToEvents()
    {
        EventMessenger.Instance().SubscribeToEvent(this, Assets.Scripts.EventSystem.Event.PLACE_LISTENING_DEVICE);
        EventMessenger.Instance().SubscribeToEvent(this, Assets.Scripts.EventSystem.Event.SPEECH_START);
        EventMessenger.Instance().SubscribeToEvent(this, Assets.Scripts.EventSystem.Event.LISTENING_DESK_OFF);
    }
}
