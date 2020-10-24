using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public bool singletone = true;
    public static MyDontDestroyOnLoad dontDestroyOnLoad;
    private void Awake()
    {
        if (singletone)
        {
            if (dontDestroyOnLoad == null)
            {
                dontDestroyOnLoad = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
