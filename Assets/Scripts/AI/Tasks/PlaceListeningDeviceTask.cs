using UnityEngine;
using Assets.Scripts.AI.TaskData;
using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using Event = Assets.Scripts.EventSystem.Event;
using System.Collections.Generic;

namespace Assets.Scripts.AI.Tasks
{
    public class PlaceListeningDeviceTask : ITask
    {
        private PathfindData _pathfindData;
        private bool _completed;
        private bool _movementNodeGenerated;
        List<GameObject> TechnicianList = new List<GameObject>();

        public PlaceListeningDeviceTask(PathfindData pathfindData)
        {
            _pathfindData = pathfindData;
        }

        public void Execute()
        {
            TechnicianList = GameManager.Instance().GetTechList();
            for (int i = 0; i < TechnicianList.Count; i++)
            {
                if (TechnicianList[i].GetComponent<Technician>().IsActive == true)
                {
                    float distance = _pathfindData.Location.x - TechnicianList[i].transform.position.x;
                    if (distance <= 2.0f && !IsComplete())
                    {
                        PlaceInRoom(TechnicianList[i].GetComponent<Character2D>().CurrentRoom, TechnicianList[i].transform.position);
                        _completed = true;
                    }
                }
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
            GameManager gameManager = GameManager.Instance();
            if (gameManager.FundingAmount >= 400)
                gameManager.FundingAmount -= 400;
            else
                return;
            GameObject listeningDevice = Resources.Load<GameObject>("ListeningDevice");
            Vector3 placementPosition = new Vector3(position.x, position.y - 0.83f, position.z);
            listeningDevice = Object.Instantiate(listeningDevice, placementPosition, Quaternion.identity);
            listeningDevice.GetComponent<ListeningDevice>().CurrentRoom = room;

            gameManager.ListeningDevList.Add(listeningDevice);

            ListeningDevicePlacedPacket eventPacket = new ListeningDevicePlacedPacket
            {
                Device = listeningDevice,
                PlacedRoom = room
            };
            EventMessenger.Instance().FireEvent(Event.LISTENING_DEVICE_PLACED, eventPacket);
        }
    }
}