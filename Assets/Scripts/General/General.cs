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
    public class General : MonoBehaviour, IEventListener
    {
        private System.Random _randomGenerator;

        private int _trust;
        private int _knowledge;
        private int _perception;
        
        private float _needsCooldown;
        private NeedStatus _bladder;
        private NeedStatus _rest;
        private NeedStatus _social;
        private NeedStatus _entertainment;

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
            _bladder = new NeedStatus("Bladder", (float)_randomGenerator.NextDouble() * 0.8f + 0.2f, 0.06f);
            _rest = new NeedStatus("Rest", (float)_randomGenerator.NextDouble() * 0.2f + 0.8f, 0.03f);
            _social = new NeedStatus("Social", (float)_randomGenerator.NextDouble(), 0.25f);
            _entertainment = new NeedStatus("Entertainment", (float)_randomGenerator.NextDouble(), 0.2f);
        }

        public void Start ()
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

                _bladder.Degrade();
                _rest.Degrade();
                _social.Degrade();

                double randomNumber = _randomGenerator.NextDouble();
                if (randomNumber > Math.Pow(_bladder.Status, 0.1) && _bladder.IsPendingRelief() == false)
                    SatisfyBladder();
                else if (randomNumber > Math.Pow(_rest.Status, 0.1) && _rest.IsPendingRelief() == false)
                    SatisfyRest();
                else if (randomNumber > Math.Pow(_social.Status, 0.1) && _social.IsPendingRelief() == false)
                    SatisfySocial();
                else if (randomNumber > Math.Pow(_entertainment.Status, 0.1) && _entertainment.IsPendingRelief() == false)
                    SatisfyEntertainment();
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

        private void SatisfyBladder()
        {
            AITaskManager.GoToToilet(this.gameObject, this._bladder);
            _bladder.SetPendingRelief();
        }

        private void SatisfyRest()
        {
            // Chance to sleep increases as rest gets low
            float chanceToSleep = (float)_randomGenerator.NextDouble();

            if (chanceToSleep > _rest.Status)
            {
                AITaskManager.GoToBed(this.gameObject, this._rest);
                _rest.SetPendingRelief();
            }
            else
            {
                AITaskManager.SitDown(this.gameObject, this._rest);
                _rest.SetPendingRelief();
            }
        }

        private void SatisfySocial()
        {
            AITaskManager.AwaitConversation(this.gameObject, this._social);
            _social.SetPendingRelief();
        }

        private void SatisfyEntertainment()
        {
            AITaskManager.LookAtArt(this.gameObject, this._entertainment);
            _entertainment.SetPendingRelief();
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

        void Inform() // pass value or script when general has conversation - list of known devices -R.Walters
        {
            //Does this character know about these listening devices if they do then don't inform-R.Walters
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

        public void SatisfyBiggestNeed()
        {
            List<NeedStatus> needs = new List<NeedStatus>
            {
                _rest,
                _bladder,
                _entertainment,
                _social
            };

            string nameOfBiggestNeed = needs.Aggregate((status1, status2) => status2.IsPendingRelief() && status1.Status > status2.Status ? status1 : status2).Name;

            switch (nameOfBiggestNeed)
            {
                case "Rest":
                    SatisfyRest();
                    break;
                case "Social":
                    SatisfySocial();
                    break;
                case "Entertainment":
                    SatisfyEntertainment();
                    break;
                case "Bladder":
                    SatisfyBladder();
                    break;
            }
        }
    }
}