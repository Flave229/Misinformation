using System;
using System.Collections.Generic;
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

        List<GameObject> SeenListeningDevices = new List<GameObject>();
        List<GameObject> GeneralsList = new List<GameObject> ();

        public Name Name;

        public General()
        {
        }

        public void Start ()
        {
            Name = NameGenerator.GenerateName();
            GeneralsList = GameManager.Instance().GetGenList();
            //GeneralsList.Add(this);
            SubscribeToEvents();
        }
	
        void Update ()
        {
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

        void Awake()
        {
            _trust = UnityEngine.Random.Range(0, 5);
            _knowledge = UnityEngine.Random.Range(0, 5);
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
    }
}