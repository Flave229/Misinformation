using System;
using Assets.Scripts.AI.TaskData;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    internal class SitTask : ITask
    {
        private DateTime _startingTime;
        private SitData _sitData;
        private bool _sitting;
        private int _secondsSitting;
        private bool _completed;

        public SitTask(SitData sitData)
        {
            _sitData = sitData;
        }

        public void Execute()
        {
            if (_sitting == false)
            {
                _startingTime = DateTime.Now;
                _sitting = true;

                if (_sitData.General.FacingRight != _sitData.Chair.FacingRight)
                {
                    Vector3 scale = _sitData.General.transform.localScale;
                    scale.x *= -1;
                    _sitData.General.transform.localScale = scale;
                    _sitData.General.FacingRight = _sitData.Chair.FacingRight;
                }
                _sitData.General.Animator.SetBool("SITTING", true);
                System.Random random = new System.Random();
                _secondsSitting = random.Next(5, 24);
            }

            if (_startingTime.AddSeconds(_secondsSitting) >= DateTime.Now)
                return;

            _sitData.General.Animator.SetBool("SITTING", false);
            _sitData.Chair.Occupied = false;
            SetCompleted();
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
            _sitData.RestNeed.Replenish(0.1f);
        }
        
        public double GetPriority()
        {
            return _sitData.RestNeed.Status;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }
    }
}