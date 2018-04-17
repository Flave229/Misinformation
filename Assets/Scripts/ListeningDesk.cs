using Assets.Scripts.EventSystem;
using UnityEngine;
using Assets.Scripts.EventSystem.EventPackets;

namespace Assets.Scripts
{
    public class ListeningDesk : MonoBehaviour, IEventListener
    {
		public int NumOfListeningDevices;

		private int activeDeviceNum;
		private GameObject activeDevice;
		private bool usingDesk = false;
        private Technician _listeningTechnician = null;

        void Start () 
		{
            SubscribeToEvents();
		}
		
		void Update () 
		{
			NumOfListeningDevices = GameManager.Instance().ListeningDevList.Count;
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
				if (NumOfListeningDevices == 0)
					LeaveDesk ();
				else
					UseDesk ();
			}
		}

		void UseDesk()
		{
            if (activeDevice != null || NumOfListeningDevices == 0)
                return;

            _listeningTechnician = GameManager.Instance().ActiveTech.GetComponent<Technician>();
            GameManager.Instance().SetUsingDesk(true);
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
			if (activeDeviceNum == (NumOfListeningDevices - 1))
				activeDeviceNum = 0;
			else
				activeDeviceNum++;
            
			if (GameManager.Instance().ListeningDevList [activeDeviceNum] != null)
				activeDevice = GameManager.Instance().ListeningDevList [activeDeviceNum];
			else
            { 
				if (activeDeviceNum == (NumOfListeningDevices - 1))
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
                TechnicianListening = _listeningTechnician,
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

            GameManager.Instance().SetUsingDesk(false);
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
                    ListeningDevicePacket packet = new ListeningDevicePacket
                    {
                        Device = activeDevice.GetComponent<ListeningDevice>(),
                        TechnicianListening = _listeningTechnician
                    };
                    EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_LISTENING, packet);
                    break;
            }
        }

        public void SubscribeToEvents()
        {
            EventMessenger.Instance().SubscribeToEvent(this, EventSystem.Event.SPEECH_START);
        }
    }
}