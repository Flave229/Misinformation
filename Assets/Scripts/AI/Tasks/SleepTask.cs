using System;
using Assets.Scripts.AI.TaskData;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    internal class SleepTask : ITask
    {
        private DateTime _startingTime;
        private bool _sleeping;
        private int _secondsSleeping;
        private bool _completed;
        private SleepData _sleepData;

        public SleepTask(SleepData dataPacket)
        {
            _sleepData = dataPacket;
        }

        public void Execute()
        {
            if (_sleeping == false)
            {
                _startingTime = DateTime.Now;
                _sleeping = true;

                if (_sleepData.General.FacingRight != _sleepData.Bed.FacingRight)
                {
                    Vector3 scale = _sleepData.General.transform.localScale;
                    scale.x *= -1;
                    _sleepData.General.transform.localScale = scale;
                }

                _sleepData.General.Animator.SetBool("SLEEPING", true);
                _sleepData.Bed.Occupied = true;
                System.Random random = new System.Random();
                _secondsSleeping = random.Next(45, 75);
                SoundManager.Instance().PlaySingleDistance(_sleepData.Bed.gameObject, "Snoring-Popup_Pixels");
            }

            if (_startingTime.AddSeconds(_secondsSleeping) >= DateTime.Now)
                return;

            _sleepData.General.Animator.SetBool("SLEEPING", false);
            _sleepData.Bed.Occupied = false;
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

        public double GetPriority()
        {
            return _sleepData.RestNeed.Status;
        }
    }
}