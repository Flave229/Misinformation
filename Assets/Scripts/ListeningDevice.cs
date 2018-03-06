using Assets.Scripts.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ListeningDevice : MonoBehaviour
    {
		public GameObject toolTip;
        public FollowMouse toolTipText;
        private int durability;
        private float dPeriod = 100;
        private double _quality;
        private GameObject _player;
        private Technician _technician;
        public Room CurrentRoom;
		public bool activeDevice = false;

        public void Start ()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _technician = _player.GetComponent<Technician>();
            durability = 2;
			toolTip = GameObject.Find ("HoverText");
            toolTipText = toolTip.GetComponent<FollowMouse>();
            _quality = (double)((_technician.GetEquipmentSkill() + 1)) / 10;
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
        }

		void OnMouseOver()
		{
            toolTipText.UpdateText ("Listening Device\n" + "Durability = " + GetDurability () + "\nQuality = " + (GetQuality() * 10) );
		}
		void OnMouseExit()
		{
            toolTipText.isEntered = false;
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
            toolTipText.UpdateText ("");
            GameManager.Instance ().ListeningDevList.Remove (this.gameObject);
            EventMessenger.Instance().FireEvent(EventSystem.Event.LISTENING_DEVICE_DESTROYED, this);
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