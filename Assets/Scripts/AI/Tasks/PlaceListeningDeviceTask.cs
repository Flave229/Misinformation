using UnityEngine;
using Assets.Scripts.AI.TaskData;
using Assets.Scripts.EventSystem;
using Assets.Scripts.EventSystem.EventPackets;
using Event = Assets.Scripts.EventSystem.Event;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.AI.Tasks
{
    public class PlaceListeningDeviceTask : ITask
    {
        private PlaceListeningDeviceData _placeListeningDeviceData;
        private bool _completed;
        private bool _movementNodeGenerated;
        List<GameObject> TechnicianList = new List<GameObject>();

        public PlaceListeningDeviceTask(PlaceListeningDeviceData placeListeningDeviceData)
        {
            _placeListeningDeviceData = placeListeningDeviceData;
        }

        public void Execute()
        {
            float distance = Math.Abs(_placeListeningDeviceData.Location.x - _placeListeningDeviceData.PlacedBy.transform.position.x);
            if (distance <= 2.0f && !IsComplete())
            {
                PlaceInRoom(_placeListeningDeviceData.PlacedBy.GetComponent<Character2D>().CurrentRoom, _placeListeningDeviceData.PlacedBy.transform.position);
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
            listeningDevice = UnityEngine.Object.Instantiate(listeningDevice, placementPosition, Quaternion.identity);
            listeningDevice.GetComponent<ListeningDevice>().CurrentRoom = room;

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