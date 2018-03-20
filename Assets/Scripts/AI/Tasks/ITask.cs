namespace Assets.Scripts.AI.Tasks
{
    public interface ITask
    {
        void Execute();
        void SetCompleted();
        bool IsComplete();
        double GetPriority();
        TaskPriorityType GetPriorityType();
        void Pause();
        void UnPause();
    }
}