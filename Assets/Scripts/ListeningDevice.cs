using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event = Assets.Scripts.EventSystem.Event;


namespace Assets.Scripts
{
    public class ListeningDevice : MonoBehaviour
    {
		public GameObject toolTip;
        private int durability;
        private float dPeriod = 10;
        private double _quality;
        private GameObject _player;
        private Technician _technician;

        public void Start ()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _technician = _player.GetComponent<Technician>();
            durability = 2;
			      toolTip = GameObject.Find ("HoverText");
            _quality = (double)((_technician.GetEquipmentSkill() + 1)) / 10;
            Debug.Log("Quality = " + _quality);

        }
	
        public void Update ()
        {
            if (durability <= 0)
                DestroyDevice();

            if (dPeriod <= 0)
            {
                if(durability != 0)
                {
                    Degrade();
                    ResetDPeriod();
                }
            }
            else
            {
                dPeriod -= Time.deltaTime;
            }
            Debug.Log("Quality = " + _quality);
        }

		void OnMouseOver()
		{
			toolTip.GetComponent<FollowMouse>().UpdateText ("Listening Device\n" + "Durability = " + GetDurability () + "\nQuality = " + (GetQuality() * 10) );
		}
		void OnMouseExit()
		{
			toolTip.GetComponent<FollowMouse>().isEntered = false;
		}

        private void ResetPeriod()
        {
            dPeriod = 10;
        }

        public void Degrade()
        {
            durability--;
        }

        public void DestroyDevice()
        {
			toolTip.GetComponent<FollowMouse> ().UpdateText ("");
            Destroy(transform.gameObject);
        }

        public void ResetDPeriod()
        {
            dPeriod = 10;
        }

        public double GetQuality()
        {
            return _quality;
        }
        
		public int GetDurability()
		{
			return durability;
		}
    }
}