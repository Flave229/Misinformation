using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;

namespace Assets.Scripts
{
    public class ListeningDeviceUI : MonoBehaviour, IEventListener
    {

        public int numOfListeningDevices;
        public Image[] listeningDeviceImages;
        Image activeImage;
        int activeNum;

        void Start()
        {
            SubscribeToEvents();
            listeningDeviceImages = GetComponentsInChildren<Image>();
            for (int i = 0; i < listeningDeviceImages.Length; ++i)
            {
                listeningDeviceImages[i].enabled = false;
            }
        }



        public void HighlightSelectedDevice(ListeningDevicePacket listeningDeviceData)
        {
            if (activeImage != null)
            {
                activeImage.GetComponent<Outline>().enabled = false;
            }
            if (listeningDeviceData != null)
            {
                activeNum = listeningDeviceData.Num;
                activeImage = listeningDeviceImages[activeNum];
                activeImage.GetComponent<Outline>().enabled = true;
            }
            
            if (activeNum > numOfListeningDevices)
            {
                activeNum = numOfListeningDevices;
            }
        }

        public void UnhighlightDevice(object eventPacket)
        {
            activeImage.GetComponent<Outline>().enabled = false;
        }
        
        public void UpdateUI(object eventPacket)
        {
            numOfListeningDevices = GameManager.Instance().GetListeningDevices().Count;
            for (int i = 0; i < numOfListeningDevices; ++i)
            {
                listeningDeviceImages[i].enabled = true;
            }
            for (int i = numOfListeningDevices; i < listeningDeviceImages.Length; ++i)
            {
                listeningDeviceImages[i].enabled = false;
            }
        }

        public void SubscribeToEvents()
        {
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DEVICE_PLACED);
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DEVICE_DESTROYED);
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DEVICE_CYCLED);
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.LISTENING_DESK_OFF);
        }

        public void ConsumeEvent(EventSystem.Event subscribeEvent, object eventPacket)
        {

            switch (subscribeEvent)
            {
                case EventSystem.Event.LISTENING_DEVICE_PLACED:
                    UpdateUI(eventPacket);
                    break;
                case EventSystem.Event.LISTENING_DEVICE_DESTROYED:
                    UpdateUI(eventPacket);
                    HighlightSelectedDevice(null);
                    break;
                case EventSystem.Event.LISTENING_DEVICE_CYCLED:
                    ListeningDevicePacket listeningDeviceData = (ListeningDevicePacket)eventPacket;
                    HighlightSelectedDevice(listeningDeviceData);
                    break;
                case EventSystem.Event.LISTENING_DESK_OFF:
                    UnhighlightDevice(eventPacket);
                    break;
            }
        }

    }
}
