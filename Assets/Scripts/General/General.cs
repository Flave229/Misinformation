using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI;
using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using Assets.Scripts.FileIO;
using UnityEngine;
using Event = Assets.Scripts.EventSystem.Event;

namespace Assets.Scripts.General
{
    public class General : MonoBehaviour, IEventListener, IGeneral
    {
        private System.Random _randomGenerator;

        private int _trust;
        private int _knowledge;
        private int _perception;

        private string ObjectivePlace;
        private string ObjectiveEvent;
        private string ObjectiveTime;

        private float _needsCooldown;
        private Dictionary<NeedType, NeedStatus> _needs;

        private List<GameObject> SeenListeningDevices = new List<GameObject>();
        
        public Name Name;

        public General()
        {
        }

        void Awake()
        {
            _randomGenerator = new System.Random();
            _perception = _randomGenerator.Next(0, 10);
            _trust = _randomGenerator.Next(0, 5);
            _knowledge = _randomGenerator.Next(0, 5);
            _needs = new Dictionary<NeedType, NeedStatus>
            {
                { NeedType.BLADDER, new NeedStatus((float)_randomGenerator.NextDouble() * 0.8f + 0.2f, 0.06f) },
                { NeedType.REST, new NeedStatus((float)_randomGenerator.NextDouble() * 0.2f + 0.8f, 0.03f) },
                { NeedType.SOCIAL, new NeedStatus((float)_randomGenerator.NextDouble(), 0.25f) },
                { NeedType.ENTERTAINMENT, new NeedStatus((float)_randomGenerator.NextDouble(), 0.2f) },
            };
        }

        public void Start()
        {
            Name = NameGenerator.GenerateName();
            SubscribeToEvents();
        }
	
        void Update()
        {
            _needsCooldown -= Time.deltaTime;

            if (_needsCooldown < 0)
            {
                _needsCooldown = 1;
                double randomNumber = _randomGenerator.NextDouble();

                foreach (var nameNeed in _needs)
                {
                    NeedStatus need = nameNeed.Value;
                    need.Degrade();

                    if (randomNumber < Math.Pow(need.Status, 0.1))
                        continue;
                    if (need.IsPendingRelief())
                        continue;
                    
                    switch (nameNeed.Key)
                    {
                        case NeedType.BLADDER:
                            SatisfyBladder(nameNeed.Value);
                            break;
                        case NeedType.ENTERTAINMENT:
                            SatisfyEntertainment(nameNeed.Value);
                            break;
                        case NeedType.REST:
                            SatisfyRest(nameNeed.Value);
                            break;
                        case NeedType.SOCIAL:
                            SatisfySocial(nameNeed.Value);
                            break;
                    }
                    break;
                }
            }
            
            for(int i = 0; i < SeenListeningDevices.Count; ++i)
            {
                if(SeenListeningDevices[i] == null)
                {
                    SeenListeningDevices.RemoveAt(i);
                    --i;
                }
            }

        }

        private void SatisfyBladder(NeedStatus need)
        {
            AITaskManager.GoToToilet(this.gameObject, need);
            need.SetPendingRelief();
        }

        private void SatisfyRest(NeedStatus need)
        {
            // Chance to sleep increases as rest gets low
            float chanceToSleep = (float)_randomGenerator.NextDouble();

            if (chanceToSleep > need.Status)
            {
                AITaskManager.GoToBed(this.gameObject, need);
                need.SetPendingRelief();
            }
            else
            {
                AITaskManager.SitDown(this.gameObject, need);
                need.SetPendingRelief();
            }
        }

        private void SatisfySocial(NeedStatus need)
        {
            AITaskManager.LookForConversation(this.gameObject, need);
            need.SetPendingRelief();
        }

        private void SatisfyEntertainment(NeedStatus need)
        {
            AITaskManager.LookAtArt(this.gameObject, need);
            need.SetPendingRelief();
        }

        public void UpdateTrustValue(int trustDifference)
        {
            _trust += trustDifference;

            if (_trust < 1)
                _trust = 1;
            else if (_trust > 10)
                _trust = 10;
        }

        public void UpdateKnowledgeValue(int knowledgeDifference)
        {
            _knowledge += knowledgeDifference;

            if (_knowledge < 1)
                _knowledge = 1;
            else if (_knowledge > 10)
                _knowledge = 10;
        }

        public void Informed(List<GameObject> otherGeneralsKnownList)
        {
            if(SeenListeningDevices.Count == 0)
            {
                SeenListeningDevices = otherGeneralsKnownList;
            }
            else
            {
                int seenCount = SeenListeningDevices.Count;
                for (int i = 0; i < otherGeneralsKnownList.Count; ++i)
                {
                    bool notThere = true;
                    for (int j = 0; j < seenCount; ++j)
                    {
                        if(otherGeneralsKnownList[i] == SeenListeningDevices[j])
                        {
                            j += seenCount;
                            notThere = false;
                        }
                    }
                    if(notThere == true)
                    {
                        SeenListeningDevices.Add(otherGeneralsKnownList[i]);
                    }
                }
            }
        }

        public List<GameObject> knowenListeringDevices()
        {
            return SeenListeningDevices;
        }

        public void ConsumeEvent(Event subscribeEvent, object eventPacket)
        {
            switch (subscribeEvent)
            {
                case Event.LISTENING_DEVICE_PLACED:
                    ListeningDevicePlacedPacket listeningDeviceData = (ListeningDevicePlacedPacket)eventPacket;
                    if (listeningDeviceData.PlacedRoom == transform.GetComponent<Character2D>().CurrentRoom)
                    {
                        SeenListeningDevices.Add(listeningDeviceData.Device);
                        Debug.Log(Name.FirstName + " " + Name.LastName + " spotted a listening device!");
                        UpdateTrustValue(((int)(UnityEngine.Random.value * -2)) - 1);
                    }

                    break;
            }
        }

        public void SubscribeToEvents()
        {
            EventMessenger.Instance().SubscribeToEvent(this, Event.LISTENING_DEVICE_PLACED);
        }
    
        public int GetKnowledge()
        {
            return _knowledge;
        }

        public int GetTrust()
        {
            return _trust;
        }

        public int GetPerception()
        {
            return _perception;
        }

        public NeedStatus GetNeed(NeedType needType)
        {
            return _needs[needType];
        }

        public NeedType GetLowestNeed()
        {
            return _needs.Aggregate((status1, status2) => status1.Value.Status > status2.Value.Status ? status1 : status2).Key;
        }

        public void SatisfyBiggestNeed()
        {
            var biggestNeed = _needs.Aggregate((status1, status2) => status1.Value.IsPendingRelief() == false && status1.Value.Status > status2.Value.Status ? status1 : status2);

            switch (biggestNeed.Key)
            {
                case NeedType.BLADDER:
                    SatisfyBladder(biggestNeed.Value);
                    break;
                case NeedType.ENTERTAINMENT:
                    SatisfyEntertainment(biggestNeed.Value);
                    break;
                case NeedType.REST:
                    SatisfyRest(biggestNeed.Value);
                    break;
                case NeedType.SOCIAL:
                    SatisfySocial(biggestNeed.Value);
                    break;
            }

        }
        public void GeneralObjectiveKnowlage()
        {
            //deturamn what the gernal thinks is ture
        }

        public string GetObjectivePlace()
        {
            return ObjectivePlace;
        }
        public string GetObjectiveEvent()
        {
            return ObjectiveEvent;
        }
        public string GetObjectiveTime()
        {
            return ObjectiveTime;
        }

        public Character2D GetCharacter()
        {
            return this.GetComponent<Character2D>();
        }
    }
}