using System;
using System.Collections.Generic;
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
        private int _trust;
        private int _knowledge;
        private int _perception;

        private float _needsCooldown;
        private NeedStatus _bladder;
        private NeedStatus _rest;
        private NeedStatus _social;

        List<GameObject> SeenListeningDevices = new List<GameObject>();
        
        public Name Name;

        public General()
        {
        }

        void Awake()
        {
            System.Random random = new System.Random();
            _perception = random.Next(0, 10);
            _trust = random.Next(0, 5);
            _knowledge = random.Next(0, 5);
            _bladder = new NeedStatus((float)random.NextDouble() * 0.8f + 0.2f, 0.06f);
            _rest = new NeedStatus((float)random.NextDouble() * 0.2f + 0.8f, 0.03f);
            _social = new NeedStatus((float)random.NextDouble(), 0.25f);
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

                System.Random randomGenerator = new System.Random();
                if (randomGenerator.NextDouble() > Math.Pow(_bladder.Status, 0.1))
                {
                    AITaskManager.GoToToilet(this.gameObject);
                    _bladder.Replenish();
                }
                if (randomGenerator.NextDouble() > Math.Pow(_rest.Status, 0.1))
                {
                    // TODO: chance to sit down or sleep
                    AITaskManager.GoToBed(this.gameObject);
                    _rest.Replenish();
                }
                if (randomGenerator.NextDouble() > Math.Pow(_social.Status, 0.1))
                {
                    AITaskManager.AwaitConversation(this.gameObject);
                    _social.Replenish();
                }
            }
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
    }
}