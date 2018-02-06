using Assets.Scripts.AI.TaskData;
using Assets.Scripts.Conversation;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;

namespace Assets.Scripts.AI.Tasks
{
    public class ConverseTask : ITask, IEventListener
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
				SoundManager.Instance().PlaySingleDistance(_converseData.General.gameObject, "generalConversation1", 1.0f, 10.0f);
				SoundManager.Instance().PlaySingleDistance(_converseData.ConversationPartnerTaskData.General.gameObject, "generalConversation2", 1.0f, 10.0f);
                _converseData.Speech = conversation[_converseData.General];
                _converseData.ConversationPartnerTaskData.Speech = conversation[_converseData.ConversationPartnerTaskData.General];
            }

            if (_converseData.Talking)
            {
                Debug.Log(_converseData.Speech);
                _timeTalking -= Time.deltaTime;

                if (_timeTalking <= 0.0f)
                {
                    _converseData.Talking = false;
                    _converseData.Done = true;
                    //_speechBubble.transform.Find("TextName").gameObject.SetActive(false);
                    //_speechBubble.transform.Find("Dialogue").gameObject.SetActive(false);
                }
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

        public void ConsumeEvent(EventSystem.Event subscribeEvent, object eventPacket)
        {
            if (_converseData.Talking == false)
                return;

            // Consume Event for ListeningDevice
            switch(subscribeEvent)
            {
                //case EventSystem.Event.LISTENING_DEVICE_LISTENING:
                case EventSystem.Event.LISTENING_DEVICE_PLACED:
                    //ListeningDeviceListeningPacket listeningDevicePacket = (ListeningDeviceListeningPacket)eventPacket;

                    //if (listeningDevicePacket.ListeningRoom != _converseData.General.gameObject.GetComponent<Character2D>().CurrentRoom)
                    //    return;

                    _speechBubble.transform.Find("Dialogue").GetComponent<Text>().text = _converseData.Speech;
                    _speechBubble.transform.Find("TextName").GetComponent<Text>().text = _converseData.General.Name.FullName();
                    _speechBubble.transform.Find("TextName").gameObject.SetActive(true);
                    _speechBubble.transform.Find("Dialogue").gameObject.SetActive(true);
                    break;
            }
        }

        public void SubscribeToEvents()
        {
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DEVICE_LISTENING);
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DEVICE_PLACED);
        }
    }
}