using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStack : MonoBehaviour
{

    //public Stack<GameObject> taskStack;
    public Queue<GameObject> taskQueue;

	void Start ()
    {
        taskQueue = new Queue<GameObject>();
	}

	void Update ()
    {

	}

    public void PopTask()
    {
        taskQueue.Dequeue();
    }

    public void PushTask()
    {
        //taskQueue.Enqueue(//addHere Needs to inherit);
    } 
}
