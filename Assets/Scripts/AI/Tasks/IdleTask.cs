using Assets.Scripts.AI.TaskData;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    public class IdleTask : ITask
    {
        private GameObject _general;
        private bool _complete;
        private bool _pause;

        public IdleTask(IdleData dataPacket)
        {
            _general = dataPacket.General;
        }

        public void Execute()
        {
            Character2D character = _general.GetComponent<Character2D>();
            character.Tasks.AddToStack(new IdleTask(new IdleData
            {
                General = _general
            }));

            General.General general = _general.GetComponent<General.General>();
            general.SatisfyBiggestNeed();
            SetCompleted();
        }

        public bool IsComplete()
        {
            return _complete;
        }

        public void SetCompleted()
        {
            if (_pause)
                return;
            _complete = true;
        }

        public bool GetCeilingLock()
        {
            return false;
        }

        public double GetPriority()
        {
            return 0;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }

        public void Pause()
        {
            _pause = true;
        }

        public void UnPause()
        {
            _pause = false;
        }
    }
}
