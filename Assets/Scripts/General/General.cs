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
        enum Trust
        {
            Trust1,
            Trust2,
            Trust3,
            Trust4,
            Trust5,
            Trust6,
            Trust7,
            Trust8,
            Trust9,
            Trust10,
            NumberOfTypes
        };

        enum Knowledge
        {
            Knowledge1,
            Knowledge2,
            Knowledge3,
            Knowledge4,
            Knowledge5,
            Knowledge6,
            Knowledge7,
            Knowledge8,
            Knowledge9,
            Knowledge10,
            NumberOfTypes
        };

        Trust TrustLevel;
        Trust newTrustLevel;
        Knowledge KnowledgeLevel;

        List<GameObject> SeenListeningDevices = new List<GameObject>();
        List<GameObject> GeneralsList = new List<GameObject> ();

        public Name Name;

        //Random speed of 0.15 to 0.25

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
            //GameObject.FindObjectsOfType(typeof(ListeningDevice)

            // if ()
            // UpdateTrustValue(TrustLevel, newTrustLevel); // Commented out currently as nothing needs to or anything to update it yet - needs to be linked to AI. - R.Walters

        }

        void UpdateTrustValue(Trust oldTrustLevel, Trust newTrustLevel)
        {
            if (oldTrustLevel != newTrustLevel)
            {
                oldTrustLevel = newTrustLevel;  // if they don't match then change them otherwise do nothing to the TrustLevel value - R.Walters
            }

            //Could have this just decay over time or change based on whether they have seen a listening device or not. - R.Walters
        }

        void Inform() // pass value or script when general has conversation - list of known devices -R.Walters
        {
            //Does this character know about these listening devices if they do then don't inform-R.Walters
        }

        void Awake()
        {
            TrustLevel = (Trust)UnityEngine.Random.Range(0,5);
            KnowledgeLevel = (Knowledge)UnityEngine.Random.Range(0, 5);
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
    
        public int GetKnow(){
            if (KnowledgeLevel == Knowledge.Knowledge1){ return 1;}
            if (KnowledgeLevel == Knowledge.Knowledge2){ return 2;}
            if (KnowledgeLevel == Knowledge.Knowledge3){ return 3;}
            if (KnowledgeLevel == Knowledge.Knowledge4){ return 4;}
            if (KnowledgeLevel == Knowledge.Knowledge5){ return 5;}
            if (KnowledgeLevel == Knowledge.Knowledge6){ return 6;}
            if (KnowledgeLevel == Knowledge.Knowledge7){ return 7;}
            if (KnowledgeLevel == Knowledge.Knowledge8){ return 8;}
            if (KnowledgeLevel == Knowledge.Knowledge9){ return 9;}
            if (KnowledgeLevel == Knowledge.Knowledge10){ return 10;}
            return 0;
        }

        public int GetTrust()
        {
            if (TrustLevel == Trust.Trust1) { return 1; }
            if (TrustLevel == Trust.Trust2) { return 2; }
            if (TrustLevel == Trust.Trust3) { return 3; }
            if (TrustLevel == Trust.Trust4) { return 4; }
            if (TrustLevel == Trust.Trust5) { return 5; }
            if (TrustLevel == Trust.Trust6) { return 6; }
            if (TrustLevel == Trust.Trust7) { return 7; }
            if (TrustLevel == Trust.Trust8) { return 8; }
            if (TrustLevel == Trust.Trust9) { return 9; }
            if (TrustLevel == Trust.Trust10) { return 10; }
            return 0;
        }

    }
}