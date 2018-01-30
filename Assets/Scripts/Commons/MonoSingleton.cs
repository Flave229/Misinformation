using UnityEngine;
using System.Collections;


public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance {get; private set; }

    public static IEnumerator WaitOnInstance()
    {
        while (Instance == null)
        {
            yield return null;
        }
    }

    protected virtual void Awake()
    {
		//Debug.LogWarning("MonoSingleton Instance Set!" + gameObject.name);
        Instance = this as T;
    }
}
