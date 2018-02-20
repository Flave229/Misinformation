﻿namespace Assets.Scripts.AI
{
    public class NeedStatus
    {
        private float _status;
        private readonly float _degradationPerMinute;

        public string Name { get; set; }
        public float Status
        {
            get { return _status; }
            set { _status = value.Clamp(0, 1); }
        }

        public NeedStatus(string name, float initialStatus, float degradationPerMinute)
        {
            Name = name;
            Status = initialStatus;
            _degradationPerMinute = degradationPerMinute;
        }

        public void Degrade()
        {
            Status -= (_degradationPerMinute / 60);
        }

        public void Replenish(float replenishAmount = 1)
        {
            Status += replenishAmount;
        }
    }
}