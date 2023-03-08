using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                GameObject temp = new GameObject(typeof(T).Name, typeof(T));
                instance = temp.GetComponent<T>();
                instance.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
            }

            return instance;
        }
    }

    private void Awake()
    {
        T[] temp = FindObjectsOfType<T>();
        if (temp.Length > 1)
            Destroy(this.gameObject);
        else
        {
            if (transform.parent != null && transform.root != null)
                DontDestroyOnLoad(this.transform.root.gameObject);
            else
                DontDestroyOnLoad(this.gameObject);
        }
    }

    public virtual void Init()
    {
        
    }
}