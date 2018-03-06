using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStack : MonoBehaviour
{
    public List<GameObject> taskList;
    public Queue<GameObject> taskQueue;
    private GameObject temp;

	void Start ()
    {
        taskQueue = new Queue<GameObject>();
	}

	void Update ()
    {

	}

    public void DequeueTask()
    {
        temp = (GameObject)taskQueue.Dequeue();
        GameObject.Destroy(temp);
        ResetQueue();
    }

    public void QueueTask(GameObject task)
    {
        temp = Instantiate(task, new Vector3(0, -(float)taskQueue.Count/2,0), taskList[0].transform.rotation);
        taskQueue.Enqueue(task);
    } 

    public void ResetQueue()
    {
        GameObject[] tempQueue = new GameObject[taskQueue.Count];
        taskQueue.CopyTo(tempQueue, 0);

        for(int y=0; y<tempQueue.Length; y++)
        {
            tempQueue[y].transform.position = new Vector2(0,-(float)y / 2);
        }
    }
}
