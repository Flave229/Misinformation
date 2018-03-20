using Assets.Scripts.AI.TaskData;
using Assets.Scripts.Conversation;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.EventSystem;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Assets.Scripts.AI.Tasks
{
    public class ConverseTask : ITask, IEventListener
    {
        private static readonly List<char> _randomCharacters = new List<char> { '#', '@', '!', '?', '/', '%', '$', '£' };

        private readonly ConverseData _converseData;
        private bool _completed;
        private readonly ConversationManager _conversationManager;

        private float _secondsToTalk;
        private readonly GameObject _speechBubble;
               
        public ConverseTask(ConverseData converseData)
        {
            _secondsToTalk = 7.0f;
            _converseData = converseData;
            _conversationManager = ConversationManager.Instance();
            _speechBubble = GameObject.FindGameObjectWithTag("ConversationPanel");
            SubscribeToEvents();
            Debug.Log("Created a converse task for " + _converseData.General.Name.FullName());
        }

        public void Execute()
        {
            if (_converseData.Done && _converseData.ConversationPartnerTaskData != null && _converseData.ConversationPartnerTaskData.Done)
                SetCompleted();

            if (_converseData.ReadyToTalk == false)
            {
                bool readyToTalk = CheckIfConversationPartnerIsNear();
                if (readyToTalk == false)
                    return;
            }

            if (_converseData.Done == false && _converseData.Talking == false && _converseData.ConversationPartnerTaskData.Talking == false)
            {
                Debug.Log(_converseData.General.Name.FullName() + " has started talking to " + _converseData.ConversationPartnerTaskData.General.Name.FullName());
                SoundManager.Instance().PlaySingleDistance(_converseData.General.gameObject, "generalConversation1", 1.0f, 10.0f);
                _converseData.Talking = true;
                EventMessenger.Instance().FireEvent(EventSystem.Event.SPEECH_START, null);
            }

            if (_converseData.Talking)
            {
                _secondsToTalk -= Time.deltaTime;

                if (_secondsToTalk <= 0.0f)
                {
                    _converseData.Talking = false;
                    _converseData.Done = true;
                    Debug.Log(_converseData.General.Name.FullName() + " has finished talking.");
                }
            }
        }

        private void GenerateConversation()
        {
            var conversation = _conversationManager.ConversationGenerator(_converseData.General, _converseData.ConversationPartnerTaskData.General);
            _converseData.Speech = conversation[_converseData.General];
            _converseData.ConversationPartnerTaskData.Speech = conversation[_converseData.ConversationPartnerTaskData.General];
        }

        private bool CheckIfConversationPartnerIsNear()
        {
            float closestGeneralDistance = float.MaxValue;
            Room generalRoom = _converseData.General.GetComponent<Character2D>().CurrentRoom;
            List<GameObject> generals = GameManager.Instance().GeneralList
                .Where(general => general != _converseData.General.gameObject 
                                    && general.GetComponent<Character2D>().CurrentRoom == generalRoom
                                    && general.GetComponent<Character2D>().Tasks.IsConcurrentCurrentTaskActive() == false).ToList();

            if (generals.Count <= 0)
                return false;

            General.General closestGeneral = generals.Aggregate((general1, general2) =>
                {
                    closestGeneralDistance = Math.Abs(general1.transform.position.x - _converseData.General.transform.position.x);
                    float generalDistance = Math.Abs(general2.transform.position.x - _converseData.General.transform.position.x);
                    if (closestGeneralDistance > generalDistance)
                        return general2;
                    return general1;
                })
                .GetComponent<General.General>();
            
            closestGeneralDistance = Math.Abs(closestGeneral.transform.position.x - _converseData.General.transform.position.x);
            if (closestGeneralDistance > 1)
                return false;

            Debug.Log(_converseData.General.Name.FullName() + " is close to " + closestGeneral.Name.FullName() + " and will try to initiate conversation");
            _converseData.General.GetComponent<Character2D>().Tasks.PauseCurrentTask();
            closestGeneral.GetComponent<Character2D>().Tasks.PauseCurrentTask();
            _converseData.ConversationPartnerTaskData = new ConverseData
            {
                ConversationPartnerTaskData = _converseData,
                General = closestGeneral,
                SocialNeed = closestGeneral.GetNeed(NeedType.SOCIAL),
                ReadyToTalk = true
            };
            closestGeneral.GetComponent<Character2D>().Tasks.AddToStack(new ConverseTask(_converseData.ConversationPartnerTaskData));

            _converseData.ReadyToTalk = true;
            GenerateConversation();
            return true;
        }

        public void SetCompleted()
        {
            Debug.Log(_converseData.General.Name.FullName() + " ending task.");
            _converseData.General.GetComponent<Character2D>().Tasks.ContinueCurrentTask();
            _completed = true;
            _converseData.SocialNeed.Replenish();
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public bool GetCeilingLock()
        {
            return false;
        }

        public void ConsumeEvent(EventSystem.Event subscribeEvent, object eventPacket)
        {
            if (_converseData.Talking == false)
                return;

            // Consume Event for ListeningDevice
            switch(subscribeEvent)
            {
                case EventSystem.Event.LISTENING_DEVICE_LISTENING:
                    ListeningDevice listeningDevice = (ListeningDevice)eventPacket;

                    if (listeningDevice.CurrentRoom != _converseData.General.gameObject.GetComponent<Character2D>().CurrentRoom)
                        return;

                    if (_converseData.Listened)
                        return;

                    _converseData.Listened = true;
                    string scrambledText = ScrambleText(listeningDevice);
                    
                    GameManager.Instance().ConversePanel.ShowPanel();
                    _speechBubble.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject.transform.Find("Dialogue02").GetComponent<Text>().text += _converseData.General.Name.FullName() + ": " + "<color=#585858ff>" + scrambledText + "</color> \n";
                    _speechBubble.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject.transform.Find("Dialogue02").gameObject.SetActive(true);
                    break;
            }
        }

        private string ScrambleText(ListeningDevice listeningDevice)
        {
            System.Random randomGenerator = new System.Random();
            
            double deviceQuality = listeningDevice.GetQuality();
            double deviceDurability = listeningDevice.GetDurability();
            double percentageTextRendered = (0.6f * deviceQuality) + (deviceQuality * deviceDurability * 0.4f);
            
            List<string> words = new List<string>(_converseData.Speech.Split(' '));

            for(int i = 0; i < words.Count; ++i)
            {
                if (randomGenerator.NextDouble() < percentageTextRendered)
                    continue;

                string scrambledString = "";

                foreach(char character in words[i])
                {
                    int index = randomGenerator.Next(0, _randomCharacters.Count);
                    scrambledString += _randomCharacters[index];
                }

                words[i] = scrambledString;
            }

            return string.Join(" ", words.ToArray());
        }

        public void SubscribeToEvents()
        {
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DEVICE_LISTENING);
        }

        public double GetPriority()
        {
            return _converseData.SocialNeed.Status;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.CONCURRENT;
        }

        public void Pause()
        {
            return;
        }

        public void UnPause()
        {
            return;
        }

        public bool IsActive()
        {
            return _converseData.ReadyToTalk;
        }
    }
}