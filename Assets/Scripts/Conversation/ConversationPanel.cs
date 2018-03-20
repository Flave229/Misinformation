using Assets.Scripts.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConversationPanel : MonoBehaviour, IEventListener
{
    public GameObject _convoPanel { get; set; }
    public GameObject _hideButton { get; set; }
    public bool panelIsHidden = true;
    public Slider refSlider { get; private set; }
    private bool sliderSeeked = false;
    List<string> storedSentences = new List<string>();


    // Use this for initialization
    void Start()
    {
        SubscribeToEvents();
        _convoPanel = GameObject.FindGameObjectWithTag("ConversationPanel");
        _hideButton = GameObject.FindGameObjectWithTag("ConversationPanel").gameObject.transform.parent.Find("ButtonHide").gameObject;
        refSlider = FindObjectOfType<Slider>();
        refSlider.minValue = 1;
        //refSlider.maxValue = 1;
        refSlider.wholeNumbers = true;
    }


    private float _cooldown = 0;

	// Update is called once per frame
	void Update ()
    {
        refSlider.onValueChanged.AddListener(ScrollStoredSentences);
        _cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.P) && _cooldown <= 0)
        {
            _cooldown = 0.5f;
            sliderSeeked = false;
            storedSentences.Add("THIS IS A DEBUG TEST THAT SHOULD NEVER EVER BE INCLUDED IN THE RELEASE BUILD Count:" + storedSentences.Count);
            int count = storedSentences.Count >= 6 ? 6 : storedSentences.Count;
            GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Text>().text = string.Join("\n", storedSentences.GetRange(0, count).ToArray());
            refSlider.maxValue = storedSentences.Count >= 6 ? storedSentences.Count - 5 : 1;
        }

        if (storedSentences.Count > 6 && !sliderSeeked)
        {
            refSlider.value = storedSentences.Count - 6;
            sliderSeeked = true;
        }
    }

    public void ScrollStoredSentences(float value)
    {
        if (value < 1)
            return;
        int startIndex = Mathf.RoundToInt(value - 1);
        int visibleSentences = storedSentences.Count - startIndex >= 6 ? 6 : storedSentences.Count - startIndex;
        GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Text>().text = string.Join("\n", storedSentences.GetRange(startIndex, visibleSentences).ToArray());
    }

    public void ShowPanel()
    {
        _hideButton.gameObject.transform.Find("TextBtn").GetComponent<Text>().text = "▼";
        PanelToggle(false);
        _hideButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _hideButton.GetComponent<Button>().onClick.AddListener(HidePanel);
    }

    public void HidePanel()
    {
        _hideButton.gameObject.transform.Find("TextBtn").GetComponent<Text>().text = "▲";
        PanelToggle(true);
        _hideButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _hideButton.GetComponent<Button>().onClick.AddListener(ShowPanel);
    }

    private void PanelToggle(bool hidePanel)
    {
        panelIsHidden = hidePanel;
        _convoPanel.SetActive(!panelIsHidden);
    }

    public void OnScrollBarDragged(Scrollbar panelScrollbar) //private before
    {

        GameObject dialogueTextObj = GameObject.FindGameObjectWithTag("Dialogue");

        dialogueTextObj.GetComponent<Text>().text = ""; 
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

    public void addSentence(string Sentence)
    {
        storedSentences.Add(Sentence);
    }

}
