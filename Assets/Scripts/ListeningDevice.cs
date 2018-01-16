using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using System.Collections.Generic;
using UnityEngine;
using Event = Assets.Scripts.EventSystem.Event;


namespace Assets.Scripts
{
    public class ListeningDevice : MonoBehaviour
    {
        private int durability;
        private float dPeriod = 10;

        public void Start ()
        {
            durability = 2;
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
            Destroy(transform.gameObject);
        }

        public void ResetDPeriod()
        {
            dPeriod = 10;
        }

    }
}