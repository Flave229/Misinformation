using UnityEngine;
using Assets.Scripts.AI.TaskData;
using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using Event = Assets.Scripts.EventSystem.Event;

namespace Assets.Scripts.AI.Tasks
{
    public class PlaceListeningDeviceTask : ITask
    {
        private PathfindData _pathfindData;
        private bool _completed;
        private bool _movementNodeGenerated;

        public PlaceListeningDeviceTask(PathfindData pathfindData)
        {
            _pathfindData = pathfindData;
        }

        public void Execute()
        {
            GameObject technician = GameObject.FindGameObjectWithTag("Player");
            float distance = _pathfindData.Location.x - technician.transform.position.x;
            if (distance <= 2.0f && !IsComplete())
            {
                PlaceInRoom(technician.GetComponent<Character2D>().CurrentRoom, technician.transform.position);
                _completed = true;
            }
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
        }

        public bool GetCeilingLock()
        {
            return false;
        }

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
    }
}