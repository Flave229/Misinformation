﻿using UnityEngine;
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
                _placeListeningDeviceData.PlacedBy.EquipmentSkill.AddExperience(500);
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

        public static void PlaceInRoom(Room room, Vector3 position)
        {
            GameManager gameManager = GameManager.Instance();
            if (gameManager.FundingAmount >= 400)
                gameManager.FundingAmount -= 400;
            else
                return;
            GameObject listeningDevice = Resources.Load<GameObject>("ListeningDevice");
            Vector3 placementPosition = new Vector3(position.x, position.y - 0.83f, position.z);
            listeningDevice = UnityEngine.Object.Instantiate(listeningDevice, placementPosition, Quaternion.identity);
            listeningDevice.GetComponent<ListeningDevice>().CurrentRoom = room;

            gameManager.ListeningDevList.Add(listeningDevice);

            ListeningDevicePlacedPacket eventPacket = new ListeningDevicePlacedPacket
            {
                Device = listeningDevice,
                PlacedRoom = room
            };
            EventMessenger.Instance().FireEvent(Event.LISTENING_DEVICE_PLACED, eventPacket);
        }

        public double GetPriority()
        {
            // TODO: Check priority on this
            return 1;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }

        public void Pause()
        {
            return;
        }

        public void UnPause()
        {
            return;
        }

        public bool IsActive()
        {
            return true;
        }
    }
}