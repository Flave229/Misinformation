using Assets.Scripts.AI.TaskData;
using Assets.Scripts.Conversation;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.EventSystem;
using System.Collections.Generic;

namespace Assets.Scripts.AI.Tasks
{
    public class ConverseTask : ITask, IEventListener
    {
        private static readonly List<char> _randomCharacters = new List<char> { '#', '@', '!', '?', '/', '%', '$', '£' };

        private readonly ConverseData _converseData;
        private bool _completed;
        private readonly ConversationManager _conversationManager;

        private float _secondsToTalk;
        private float _secondsToWait;
        private readonly GameObject _speechBubble;

       
        public ConverseTask(ConverseData converseData)
        {
            _secondsToTalk = 7.0f;
            _secondsToWait = 30.0f;
            _converseData = converseData;
            _conversationManager = ConversationManager.Instance();
            _speechBubble = GameObject.FindGameObjectWithTag("ConversationPanel");
            SubscribeToEvents();
        }

        public void Execute()
        {
            if (_converseData.ReadyToTalk == false)
                _converseData.ReadyToTalk = true;
            
            if (_converseData.Done && _converseData.ConversationPartnerTaskData.Done)
                SetCompleted();

            if (_converseData.ConversationPartnerTaskData.ReadyToTalk == false)
            {
                _secondsToWait -= Time.deltaTime;

                if (_secondsToWait <= 0.0f)
                {
                    _converseData.Done = true;
                    _converseData.ConversationPartnerTaskData.Done = true;
                }

                return;
            }

            if (_converseData.Talking == false && _converseData.Done == false && _converseData.ConversationPartnerTaskData.Talking == false)
            {
                if (_converseData.ConversationPartnerTaskData.Done == false)
                {
                    var conversation = _conversationManager.ConversationGenerator(_converseData.General, _converseData.ConversationPartnerTaskData.General);
                    _converseData.Speech = conversation[_converseData.General];
                    _converseData.ConversationPartnerTaskData.Speech = conversation[_converseData.ConversationPartnerTaskData.General];
                }

				SoundManager.Instance().PlaySingleDistance(_converseData.General.gameObject, "generalConversation1", 1.0f, 10.0f);
				SoundManager.Instance().PlaySingleDistance(_converseData.ConversationPartnerTaskData.General.gameObject, "generalConversation2", 1.0f, 10.0f);
                
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
                }
            }
        }

        public void SetCompleted()
        {
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
            return 1 - _converseData.SocialNeed.Status;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }
    }
}