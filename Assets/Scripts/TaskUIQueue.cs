using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.AI.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIQueue : MonoBehaviour
{
    public GameObject canvasGO;
    public RectTransform canvasRT;
    public Canvas canvas;

    public List<ITask>       tasksList;
    public List<GameObject>  taskUIList;
    public Queue<GameObject> taskQueue;

    public  GameObject activeTech;
    private GameObject temp;

	void Start ()
    {
        Debug.Log("Initialised");
        SetupCanvas();
        taskQueue = new Queue<GameObject>();
        activeTech = GameManager.Instance().ActiveTech;
        tasksList = activeTech.GetComponent<Character2D>().Tasks.GetTasks();

        foreach(ITask cTask in tasksList)
        {
            if(cTask.GetType() == typeof(PathfindToLocationTask))
            {
                SetupButton(cTask);
            }
            else if(cTask.GetType() == typeof(PlaceListeningDeviceTask))
            {
                SetupButton(cTask);
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

    public void SetupCanvas()
    {
        canvasGO = new GameObject();
        canvasRT = canvasGO.AddComponent<RectTransform>();
        Canvas canvas = GameObject.Find("Canvas-UI").GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Vector3 pos = Camera.main.transform.position;
        pos += Camera.main.transform.forward * 2.0f;
        canvas.worldCamera = Camera.main;
    }

    private void SetupButton(ITask task)
    {
        GameObject buttonGO = new GameObject();
        RectTransform buttonRT = buttonGO.AddComponent<RectTransform>();
        buttonRT.SetParent(canvasRT);
        buttonRT.sizeDelta = new Vector2(100.0f, 100.0f);
        Button taskButton = buttonGO.AddComponent<Button>();
        taskButton.onClick.AddListener(() => { Debug.Log("button clicked"); });
        Image buttonI = buttonGO.AddComponent<Image>();

        if (task.GetType() == typeof(PathfindToLocationTask))
        {
            buttonI.sprite = Resources.Load<Sprite>("UI/WalkBox");
        }
        else if (task.GetType() == typeof(PlaceListeningDeviceTask))
        {
            buttonI.sprite = Resources.Load<Sprite>("UI/BugBox");
        }
    }

    public void HandleClick()
    {

    }
}
