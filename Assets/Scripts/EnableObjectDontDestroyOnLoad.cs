using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        MyDontDestroyOnLoad go = FindObjectOfType<MyDontDestroyOnLoad>();
        if(go != null)
            go.gameObject.SetActive(true);
    }
}
