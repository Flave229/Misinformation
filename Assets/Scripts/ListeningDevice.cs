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

        public void Start ()
        {
            durability = 2;
			toolTip = GameObject.Find ("HoverText");
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
            Debug.Log("Durability = " + durability);
        }

		void OnMouseOver()
		{
			toolTip.GetComponent<FollowMouse>().UpdateText ("Listening Device\n" + "Durability = " + GetDurability ());
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

		public int GetDurability()
		{
			return durability;
		}
    }
}