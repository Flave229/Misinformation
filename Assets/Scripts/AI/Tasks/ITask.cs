namespace Assets.Scripts.AI.Tasks
{
    public interface ITask
    {
        void Execute();
        void SetCompleted();
        bool IsComplete();
        bool GetCeilingLock();
        double GetPriority();
    }
}