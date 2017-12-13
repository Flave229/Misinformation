using UnityEngine;
using Assets.Scripts.AI.TaskData;

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
                ListeningDevice.PlaceInRoom(technician.GetComponent<Character2D>().CurrentRoom, technician.transform.position);
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
    }
}