using System;

namespace Assets.Scripts.AI.Tasks
{
    class FindListeningDeviceTask : ITask
    {
        private readonly FindListeningDeviceData _dataPacket;

        private DateTime _startingTime;
        private bool _searching;
        private bool _completed;
        private int _secondsSearching;

        public FindListeningDeviceTask(FindListeningDeviceData dataPacket)
        {
            _dataPacket = dataPacket;
            var random = new Random();
            _secondsSearching = random.Next(6, 15);
        }

        public void Execute()
        {
            // TODO: Add animation

            if (_startingTime.AddSeconds(_secondsSearching) >= DateTime.Now)
                return;

            double generalPerception = (double)(_dataPacket.General.GetPerception()) / 10;

            if (_dataPacket.Furniture.HasListeningDevice() == false)
            { 
                SetCompleted();
                return;
            }

            double deviceQuality = _dataPacket.Furniture.GetListeningDevice().GetQuality();
            double chanceToFind = 0.25 * (0.2 * deviceQuality) * (0.25 * generalPerception);
            Random random = new Random();
            if (random.NextDouble() <= chanceToFind)
                _dataPacket.General.UpdateTrustValue(-2);
            SetCompleted();
        }


        public bool GetCeilingLock()
        {
            return false;
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
        }
    }
}
