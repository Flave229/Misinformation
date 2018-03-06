namespace Assets.Scripts.AI
{
    public class NeedStatus
    {
        private bool _pendingRelief;
        private float _status;
        private readonly float _degradationPerMinute;
        
        public float Status
        {
            get { return _status; }
            set { _status = value.Clamp(0, 1); }
        }

        public NeedStatus(float initialStatus, float degradationPerMinute)
        {
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
            _pendingRelief = false;
        }

        public void SetPendingRelief()
        {
            _pendingRelief = true;
        }

        public bool IsPendingRelief()
        {
            return _pendingRelief;
        }
    }
}