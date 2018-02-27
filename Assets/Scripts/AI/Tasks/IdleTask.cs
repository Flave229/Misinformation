using Assets.Scripts.AI.TaskData;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    public class IdleTask : ITask
    {
        private GameObject _general;
        private bool _complete;

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
    }
}
