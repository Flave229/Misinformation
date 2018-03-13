using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.AI.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIQueue : MonoBehaviour
{
    public List<ITask>       tasksList;
    public List<GameObject>  taskUIList;
    public Queue<GameObject> taskQueue;

    public  GameObject activeTech;
    private GameObject temp;

    private Image image;

	void Start ()
    {
        taskQueue = new Queue<GameObject>();
        activeTech = GameManager.Instance().ActiveTech;
        tasksList = activeTech.GetComponent<Character2D>().Tasks.GetTasks();

        foreach(ITask cTask in tasksList)
        {
            if(cTask.GetType() == typeof(PathfindToLocationTask))
            {
                //QueueTask();
            }
            else if(cTask.GetType() == typeof(PlaceListeningDeviceTask))
            {
                //QueueTask();
            }
        }
    }

	void Update ()
    {
        if(GameManager.Instance().ActiveTech != activeTech)
        {
            activeTech = GameManager.Instance().ActiveTech;
            tasksList = activeTech.GetComponent<Character2D>().Tasks.GetTasks();
        }
	}

    public void DequeueTask()
    {
        temp = (GameObject)taskQueue.Dequeue();
        GameObject.Destroy(temp);
        ResetQueue();
    }

    public void QueueTask(GameObject task)
    {
        temp = Instantiate(task, new Vector3(0, -(float)taskQueue.Count/2,0), taskUIList[0].transform.rotation);
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
