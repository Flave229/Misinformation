using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using UnityEngine;
using Event = Assets.Scripts.EventSystem.Event;

namespace Assets.Scripts
{
    public class ListeningDevice : MonoBehaviour
    {
        public static void PlaceInRoom(Room room, Vector3 position)
        {
            GameObject listeningDevice = Resources.Load<GameObject>("ListeningDevice");
            Vector3 placementPosition = new Vector3(position.x, position.y - 0.83f, position.z);
            listeningDevice = Object.Instantiate(listeningDevice, placementPosition, Quaternion.identity);

            GameManager gameManager = GameManager.Instance();
            gameManager.ListeningDevList.Add(listeningDevice);
            gameManager.FundingAmount -= 400;
            ListeningDevicePlacedPacket eventPacket = new ListeningDevicePlacedPacket
            {
                Device = listeningDevice,
                PlacedRoom = room
            };
            EventMessenger.Instance().FireEvent(Event.LISTENING_DEVICE_PLACED, eventPacket);
        }

        public void Start ()
        {

        }
	
        void Update ()
        {
		
        }
    }
}