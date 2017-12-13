using System;

namespace Assets.Scripts.AI.Tasks
{
    public class LookAtArtTask : ITask
    {
        private DateTime _startingTime;
        private bool _active;
        private int _secondsActive;
        private bool _completed;

        public void Execute()
        {
            if (_active == false)
            {
                _startingTime = DateTime.Now;
                _active = true;
                
                System.Random random = new System.Random();
                _secondsActive = random.Next(5, 15);
            }

            if (_startingTime.AddSeconds(_secondsActive) < DateTime.Now)
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

        public bool GetCeilingLock()
        {
            return false;
        }
    }
}
