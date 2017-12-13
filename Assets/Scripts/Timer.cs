using System;

namespace Assets.Scripts
{
    class Timer
    {
        private static Timer _instance;
        private DateTime _lastInspectedTime;
        private float _remainingTimeInMiliseconds;
        private int _startingTimeInSeconds;

        public bool Paused { get; set; }

        private Timer()
        {
            _lastInspectedTime = DateTime.Now;
        }

        public static Timer Instance()
        {
            if (_instance != null)
                return _instance;

            _instance = new Timer();
            return _instance;
        }

        public void SetStartingTime(int seconds)
        {
            _startingTimeInSeconds = seconds;
        }

        public int GetRemainingTime()
        {
            if (Paused)
                return (int)(_remainingTimeInMiliseconds / 1000);

            DateTime now = DateTime.Now;
            _remainingTimeInMiliseconds -= (float)(now - _lastInspectedTime).TotalMilliseconds;
            _lastInspectedTime = now;
            return (int)(_remainingTimeInMiliseconds / 1000);
        }

        public int GetStartingTimeInSeconds()
        {
            return _startingTimeInSeconds;
        }

        public void ResetRemainingTime()
        {
            _lastInspectedTime = DateTime.Now;
            _remainingTimeInMiliseconds = _startingTimeInSeconds * 1000;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Play()
        {
            _lastInspectedTime = DateTime.Now;
            Paused = false;
        }
    }
}