using System;
using Assets.Scripts.AI.TaskData;

namespace Assets.Scripts.AI.Tasks
{
    class UseToiletTask : ITask
    {
        private DateTime _startingTime;
        private int _secondsOnToilet;
        private bool _usingToilet;
        private bool _completed;
        private readonly ToiletData _toiletData;

        public UseToiletTask(ToiletData dataPacket)
        {
            _toiletData = dataPacket;
        }

        public void Execute()
        {
            if (_usingToilet == false)
            {
                _startingTime = DateTime.Now;
                _usingToilet = true;

                _toiletData.General.Animator.SetBool("USINGTOILET", true);
                System.Random random = new System.Random();
                _secondsOnToilet = random.Next(5, 24);
            }

            if (_startingTime.AddSeconds(_secondsOnToilet) >= DateTime.Now)
                return;

            _toiletData.General.Animator.SetBool("USINGTOILET", false);
            _toiletData.Toilet.Occupied = false;
			SoundManager.Instance().PlaySingleDistance(_toiletData.Toilet.gameObject, "toiletFlush");
            SetCompleted();
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
            _toiletData.BladderNeed.Replenish();
        }

        public double GetPriority()
        {
            return 1 - _toiletData.BladderNeed.Status;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }
    }
}