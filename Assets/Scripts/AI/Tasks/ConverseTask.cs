﻿using Assets.Scripts.AI.TaskData;
using Assets.Scripts.Conversation;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

namespace Assets.Scripts.AI.Tasks
{
    public class ConverseTask : ITask
    {
        private readonly ConverseData _converseData;
        private bool _completed;
        private readonly ConversationManager _conversationManager;
        private readonly float _defaultTimeTalking;
        private float _timeTalking;
        private readonly float _defaultTimeWaiting;
        private float _timeWaiting;
        private GameObject _speechBubble;


        public ConverseTask(ConverseData converseData)
        {
            _defaultTimeTalking = 5.0f;
            _timeTalking = _defaultTimeTalking;
            _defaultTimeWaiting = 30.0f;
            _timeWaiting = _defaultTimeWaiting;
            _converseData = converseData;
            _conversationManager = ConversationManager.Instance();
            _speechBubble = GameObject.FindGameObjectWithTag("ConversationPanel");
        }

        public void Execute()
        {
            if (_converseData.ReadyToTalk == false)
                _converseData.ReadyToTalk = true;
            
            if (_converseData.Done && _converseData.ConversationPartnerTaskData.Done)
                SetCompleted();

            if (_converseData.ConversationPartnerTaskData.ReadyToTalk == false)
            {
                _timeWaiting -= Time.deltaTime;

                if (_timeWaiting <= 0.0f)
                {
                    _converseData.Done = true;
                    _converseData.ConversationPartnerTaskData.Done = true;
                }

                return;
            }

            if (_converseData.Talking == false && _converseData.Done == false && _converseData.ConversationPartnerTaskData.Talking == false)
            {
                _converseData.Talking = true;
                var conversation =  _conversationManager.ConversationGenerator(_converseData.General, _converseData.ConversationPartnerTaskData.General);
                Debug.Log("A conversation started!");
				SoundManager.Instance ().PlaySingleDistance (_converseData.General.gameObject, "generalConversation1", 1.0f, 10.0f);
				SoundManager.Instance ().PlaySingleDistance (_converseData.ConversationPartnerTaskData.General.gameObject, "generalConversation2", 1.0f, 10.0f);
                _converseData.Speech = conversation[_converseData.General];
                _converseData.ConversationPartnerTaskData.Speech = conversation[_converseData.ConversationPartnerTaskData.General];
                //_speechBubble.transform.Find("DialogueField").GetComponent<InputField>().ActivateInputField();
                //_speechBubble.transform.Find("DialogueField").GetComponent<InputField>().textComponent.text += _converseData.General.Name.FullName() + ":" + _converseData.Speech + "\n";
                // _speechBubble.transform.Find("DialogueField").GetComponent<InputField>().text += _converseData.General.Name.FullName() + ":" + _converseData.Speech + "\n";
                //_speechBubble.transform.Find("Dialogue02").GetComponent<Text>().text += _converseData.General.Name.FullName() +   ": " + "<color=#585858ff>" + _converseData.Speech +  "</color> \n";
                _speechBubble.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject.transform.Find("Dialogue02").GetComponent<Text>().text += _converseData.General.Name.FullName() + ": " + "<color=#585858ff>" + _converseData.Speech + "</color> \n";
                // _speechBubble.transform.Find("TextName").GetComponent<Text>().text = _converseData.General.Name.FullName();
                // _speechBubble.transform.Find("TextName").gameObject.SetActive(true);
                //_speechBubble.transform.Find("DialogueField").gameObject.SetActive(true);
                _speechBubble.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject.transform.Find("Dialogue02").gameObject.SetActive(true);
                //_speechBubble.transform.Find("Viewport").gameObject.SetActive(true);
            }


                

            if (_converseData.Talking)
            {
                _timeTalking -= Time.deltaTime;

                if (_timeTalking <= 0.0f)
                {
                    _converseData.Talking = false;
                    _converseData.Done = true;
                    _speechBubble.transform.Find("TextName").gameObject.SetActive(false);
                    _speechBubble.transform.Find("Dialogue").gameObject.SetActive(false);
                }

                Debug.Log(_converseData.Speech);
            }
        }

        public void SetCompleted()
        {
            _completed = true;
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public bool GetCeilingLock()
        {
            return false;
        }
    }
}