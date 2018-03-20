using Assets.Scripts.EventSystem;
using UnityEngine;
using Assets.Scripts.EventSystem.EventPackets;

namespace Assets.Scripts
{
    public class ListeningDesk : MonoBehaviour, IEventListener
    {
		public int numOfListeningDevices;
		int activeDeviceNum;
		GameObject activeDevice;
		bool usingDesk = false;
        
		void Start () 
		{
            SubscribeToEvents();
		}
		
		void Update () 
		{
			numOfListeningDevices = GameManager.Instance().ListeningDevList.Count;
            if (GameManager.Instance().ActiveTech != null)
            {
                float distance = Vector3.Distance(GameManager.Instance().ActiveTech.transform.position, this.gameObject.transform.position);

                if (distance < 1)
                {
                    if (Input.GetKeyDown(KeyCode.U) && Timer.Instance().GetRemainingTime() > 0)
                    {
                        if (usingDesk == false)
                            UseDesk();
                        else if (usingDesk == true)
                            LeaveDesk();

                    }

                    if (usingDesk == true && Input.GetKeyDown(KeyCode.Y))
                        CycleDevices();
                }
            }
			if (usingDesk == true && activeDevice == null)
            {
				if (numOfListeningDevices == 0)
					LeaveDesk ();
				else
					UseDesk ();
			}
		}

		void UseDesk()
		{
            if (activeDevice != null || numOfListeningDevices == 0)
                return;
            
			activeDevice = GameManager.Instance ().ListeningDevList [0];
			activeDeviceNum = 0;
			activeDevice.GetComponent<ListeningDevice> ().activeDevice = true;
			Camera.main.GetComponent<Camera2DFollow> ().target = activeDevice.transform;
			usingDesk = true;
            ListeningDevicePacket eventPacket = new ListeningDevicePacket
            {
                Device = activeDevice.GetComponent<ListeningDevice>(),
                Num = activeDeviceNum
            };
            EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_CYCLED, eventPacket);
        }

		void CycleDevices()
		{
			activeDevice.gameObject.GetComponent<ListeningDevice>().activeDevice = false;
			if (activeDeviceNum == (numOfListeningDevices - 1))
				activeDeviceNum = 0;
			else
				activeDeviceNum++;
            
			if (GameManager.Instance().ListeningDevList [activeDeviceNum] != null)
				activeDevice = GameManager.Instance().ListeningDevList [activeDeviceNum];
			else
            { 
				if (activeDeviceNum == (numOfListeningDevices - 1))
					activeDeviceNum = 0;
				else
					activeDeviceNum++;

				activeDevice = GameManager.Instance().ListeningDevList [activeDeviceNum];
			}

            ListeningDevice newActiveDevice = activeDevice.gameObject.GetComponent<ListeningDevice>();
            newActiveDevice.activeDevice = true;
			Camera.main.GetComponent<Camera2DFollow>().target = activeDevice.transform;

            ListeningDevicePacket eventPacket = new ListeningDevicePacket
            {
                Device = newActiveDevice,
                Num = activeDeviceNum
            };

            EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_LISTENING, eventPacket);
            EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_CYCLED, eventPacket);
        }

		void LeaveDesk()
		{
			if (activeDevice != null)
            {
				activeDevice.GetComponent<ListeningDevice>().activeDevice = false;
				activeDevice = null;
			}

			usingDesk = false;
			Camera.main.GetComponent<Camera2DFollow>().target = GameManager.Instance().ActiveTech.transform;
            EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DESK_OFF, null);
        }

        public void ConsumeEvent(EventSystem.Event subscribeEvent, object eventPacket)
        {
            if (activeDevice == null)
                return;

            switch (subscribeEvent)
            {
                case EventSystem.Event.SPEECH_START:
                    EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_LISTENING, activeDevice.GetComponent<ListeningDevice>());
                    break;
            }
        }

        public void SubscribeToEvents()
        {
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.SPEECH_START);
        }
    }
}