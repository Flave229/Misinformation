using Assets.Scripts.AI.Tasks;
using System.Collections.Generic;

namespace Assets.Scripts.AI
{
    public class AIStack
    {
        private ITask _executingTask;
        private readonly Stack<ITask> _tasks;

        public AIStack()
        {
            _tasks = new Stack<ITask>();
        }

        public void AddToStack(ITask task)
        {
            if (_tasks.Count == 0)
                _tasks.Push(task);
            else if (_tasks.Peek().GetCeilingLock() == false)
                _tasks.Push(task);
        }

        public void Update()
        {
            if (_tasks.Count == 0 && _executingTask == null)
                return;

            if (_executingTask == null)
                _executingTask = _tasks.Pop();

            if (_executingTask.IsComplete())
            {
                if (_tasks.Count > 0)
                    _executingTask = _tasks.Pop();
                else
                {
                    _executingTask = null;
                    return;
                }
            }

            _executingTask.Execute();
        }

        public void Destroy()
        {
            if (_executingTask != null)
                _executingTask.SetCompleted();
            _tasks.Clear();
        }
    }
}
