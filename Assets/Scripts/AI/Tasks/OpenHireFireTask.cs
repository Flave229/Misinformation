using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    public class OpenHireFireTask : ITask
    {
        private GameObject _canvas;
        private bool _completed;

        public OpenHireFireTask(GameObject canvas)
        {
            _canvas = canvas;
        }

        public void Execute()
        {
            if (_canvas.activeSelf == false)
            {
                _canvas.SetActive(true);
            }
            SetCompleted();
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
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
