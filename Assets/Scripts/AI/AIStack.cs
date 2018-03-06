using Assets.Scripts.AI.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AI
{
    public class AIStack
    {
        private ITask _executingTask;
        private readonly List<ITask> _tasks;

        public AIStack()
        {
            _tasks = new List<ITask>();
        }

        public void AddToStack(ITask task)
        {
            if (_tasks.Count == 0)
                _tasks.Add(task);
            else if (_tasks[_tasks.Count - 1].GetCeilingLock() == false)
                _tasks.Add(task);
        }

        public void Update()
        {
            if (_tasks.Count == 0 && _executingTask == null)
                return;

            if (_executingTask == null)
                SelectNextTask();

            if (_executingTask.IsComplete())
            {
                if (_tasks.Count > 0)
                    SelectNextTask();
                else
                {
                    _executingTask = null;
                    return;
                }
            }

            _executingTask.Execute();
        }

        private void SelectNextTask()
        {
            ITask highestPriorityTask = _tasks.Aggregate((highestPriority, task) => task.GetPriority() > highestPriority.GetPriority() ? task : highestPriority);
            _tasks.Remove(highestPriorityTask);
            _executingTask = highestPriorityTask;
        }

        public void InterruptCurrentTask()
        {
            if (_executingTask != null)
                _executingTask.SetCompleted();
        }
        
        public void InterruptCurrentTaskIfType(Type type)
        {
            if (_executingTask != null && _executingTask.GetType() == type)
                _executingTask.SetCompleted();
        }

        public void Destroy()
        {
            if (_executingTask != null)
                _executingTask.SetCompleted();
            _tasks.Clear();
        }

        public List<ITask> GetTasksOfType()
        {
            List<ITask> currentTaskList = new List<ITask>();

            if (_executingTask.GetType() == typeof(AITaskChain))
            {
                AITaskChain taskChain = (AITaskChain)_executingTask;

                foreach(ITask task in taskChain.GetTasks())
                {
                    currentTaskList.Add(task);
                }
            }

            foreach (ITask task in _tasks)
            {
                if (task.GetType() == typeof(AITaskChain))
                {
                    AITaskChain taskChain = (AITaskChain)task;

                    foreach (ITask childTask in taskChain.GetTasks())
                    {
                        currentTaskList.Add(childTask);
                    }
                }
                else
                {
                    currentTaskList.Add(task);
                }
            }

            return currentTaskList;
        }
    }
}
