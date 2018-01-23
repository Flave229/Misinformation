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
        private double _quality;
        private GameObject _player;
        private Technician _technician;

        public void Start ()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _technician = _player.GetComponent<Technician>();
            durability = 2;
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

        public double GetQuality()
        {
            return _quality;
        }
    }
}