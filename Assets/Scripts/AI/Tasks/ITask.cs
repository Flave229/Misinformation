﻿namespace Assets.Scripts.AI.Tasks
{
    public interface ITask
    {
        void Execute();
        void SetCompleted();
        bool IsComplete();
        double GetPriority();
        TaskPriorityType GetPriorityType();
        bool IsActive();
        void Pause();
        void UnPause();
    }
}