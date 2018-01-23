using Assets.Scripts.AI.TaskData;
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
        private GameObject _speechText;

        public ConverseTask(ConverseData converseData)
        {
            _defaultTimeTalking = 5.0f;
            _timeTalking = _defaultTimeTalking;
            _defaultTimeWaiting = 30.0f;
            _timeWaiting = _defaultTimeWaiting;
            _converseData = converseData;
            _conversationManager = ConversationManager.Instance();

            _speechBubble = null;
            _speechText = null;
            //_speechText.SetActive(false);
            //_speechBubble.SetActive(false);
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
                _speechBubble = GameObject.Instantiate(Resources.Load<GameObject>("TestPrefab"));
                _speechBubble.transform.parent = _converseData.General.transform;
              //  _speechText = GameObject.Instantiate(new GameObject("SpeechText"));
              //_speechText = GameObject.Find("ConversationText");
              // _speechText.AddComponent<Text>();
              //_speechText.GetComponent<TextMesh>().text = _speechBubble.GetComponent<Text>().text;
              //_speechText.GetComponent<Text>().text = _speechBubble.GetComponent<Text>().text;
              // _speechBubble.AddComponent<TextMesh>();
              // _speechBubble.transform.parent = GameObject.Find("CanvasConversation").transform;
              //_speechBubble.
              //_speechBubble.GetComponent<Text>().text = "This is some Text/This is a Test";
              // _speechText.transform.parent = GameObject.Find(_speechBubble.name).transform;
              // _speechBubble.transform.GetChild(0).gameObject.GetComponent<Text>().text = "This is a Test";
                _speechBubble.transform.position = new Vector3(_converseData.General.transform.position.x - 0.4f, _converseData.General.transform.position.y + 1.0f, 0.0f);        
               // _speechText.transform.position = _speechBubble.transform.position;
                _speechBubble.SetActive(true);
               // _speechText.SetActive(true);
            }


                

            if (_converseData.Talking)
            {
                _timeTalking -= Time.deltaTime;

                if (_timeTalking <= 0.0f)
                {
                    _converseData.Talking = false;
                    _converseData.Done = true;
                    _speechBubble.SetActive(false);
                }

                Debug.Log(_converseData.Speech);
            }
        }

        public void SetCompleted()
        {
            if(_speechBubble != null)
                Object.Destroy(_speechBubble);

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