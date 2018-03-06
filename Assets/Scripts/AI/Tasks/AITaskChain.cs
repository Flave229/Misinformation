using System.Collections.Generic;

namespace Assets.Scripts.AI.Tasks
{
    public class AITaskChain : ITask
    {
        private ITask _executingTask;
        private readonly Stack<ITask> _tasks;
        private bool _completed;
        private bool _ceilingLock;

        public AITaskChain(Stack<ITask> tasks)
        {
            _tasks = tasks;
        }

        public void Execute()
        {
            if (_executingTask == null)
                _executingTask = _tasks.Pop();

            if (_executingTask.IsComplete())
            {
                if (_tasks.Count > 0)
                    _executingTask = _tasks.Pop();
                else
                    SetCompleted();
            }

            if (IsComplete() == false)
                _executingTask.Execute();
        }

        public void SetCompleted()
        {
            _completed = true;
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCeilingLock(bool ceilingLock)
        {
            _ceilingLock = ceilingLock;
        }

        public bool GetCeilingLock()
        {
            return _ceilingLock;
        }

        public double GetPriority()
        {
            double highestPriority = 0;
            foreach(var task in _tasks)
            {
                if (task.GetPriority() > highestPriority)
                    highestPriority = task.GetPriority();
            }

            if (_executingTask != null && _executingTask.GetPriority() > highestPriority)
                highestPriority = _executingTask.GetPriority();

            return highestPriority;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }
    }
}