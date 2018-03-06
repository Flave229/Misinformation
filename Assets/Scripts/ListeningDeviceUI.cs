using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.EventSystem;

namespace Assets.Scripts
{
    public class ListeningDeviceUI : MonoBehaviour, IEventListener
    {

        public int numOfListeningDevices;
        public Image[] listeningDeviceImages;

        void Start()
        {
            SubscribeToEvents();
            listeningDeviceImages = GetComponentsInChildren<Image>();
            for (int i = 0; i < listeningDeviceImages.Length; ++i)
            {
                listeningDeviceImages[i].enabled = false;
            }
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
                    break;
            }
        }

    }
}
