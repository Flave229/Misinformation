﻿using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using UnityEngine;

namespace Assets.Scripts
{
    public class ListeningDesk : MonoBehaviour
    {
		public int numOfListeningDevices;
		int activeDeviceNum;
		GameObject activeDevice;
		bool usingDesk = false;
        
		void Start () 
		{
			
		}
		
		void Update () 
		{
			numOfListeningDevices = GameManager.Instance().ListeningDevList.Count;
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

				if (usingDesk == true && Input.GetKeyDown (KeyCode.Y))
					CycleDevices ();
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
            EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_LISTENING, newActiveDevice);
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
		}
	}
}