using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class ListeningDesk : MonoBehaviour {

		public int numOfListeningDevices;
		int activeDeviceNum;
		GameObject activeDevice;
		bool usingDesk = false;

		// Use this for initialization
		void Start () 
		{
			
		}
		
		// Update is called once per frame
		void Update () 
		{
			numOfListeningDevices = GameManager.Instance ().ListeningDevList.Count;
			float distance = Vector3.Distance(GameManager.Instance().ActiveTech.transform.position, this.gameObject.transform.position);
			if (distance < 1)
			{
				if (Input.GetKeyDown(KeyCode.U) && Timer.Instance().GetRemainingTime() > 0)
				{
					if (usingDesk == false) {
						UseDesk ();
					} else if (usingDesk == true) {
						LeaveDesk ();
					}
				}

				if (usingDesk == true && Input.GetKeyDown (KeyCode.Y)) {
					CycleDevices ();
				}
			}
			if (usingDesk == true && activeDevice == null) {
				if (numOfListeningDevices == 0) {
					LeaveDesk ();
				} else {
					UseDesk ();
				}
			}
		}

		void UseDesk()
		{
			
			if (activeDevice == null) 
			{
				if (numOfListeningDevices != 0) {
					activeDevice = GameManager.Instance ().ListeningDevList [0];
					activeDeviceNum = 0;
					activeDevice.GetComponent<ListeningDevice> ().activeDevice = true;
					Camera.main.GetComponent<Camera2DFollow> ().target = activeDevice.transform;
					usingDesk = true;
				} else 
				{
					//no listening devices dumb dumb
				}
			}
		}

		void CycleDevices()
		{
			activeDevice.gameObject.GetComponent<ListeningDevice> ().activeDevice = false;
			if (activeDeviceNum == (numOfListeningDevices - 1)) {
				activeDeviceNum = 0;
			} else {
				activeDeviceNum++;
			}
			if (GameManager.Instance ().ListeningDevList [activeDeviceNum] != null) {
				activeDevice = GameManager.Instance ().ListeningDevList [activeDeviceNum];
			} else {
				if (activeDeviceNum == (numOfListeningDevices - 1)) {
					activeDeviceNum = 0;
				} else {
					activeDeviceNum++;
				}
				activeDevice = GameManager.Instance ().ListeningDevList [activeDeviceNum];
			}
			activeDevice.gameObject.GetComponent<ListeningDevice>().activeDevice = true;
			Camera.main.GetComponent<Camera2DFollow> ().target = activeDevice.transform;
		}

		void LeaveDesk()
		{
			if (activeDevice != null) {
				activeDevice.GetComponent<ListeningDevice> ().activeDevice = false;
				activeDevice = null;
			}
			usingDesk = false;
			Camera.main.GetComponent<Camera2DFollow> ().target = GameManager.Instance().ActiveTech.transform;
		}
	}
}
